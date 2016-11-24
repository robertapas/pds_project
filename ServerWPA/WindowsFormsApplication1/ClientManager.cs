using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace WindowsFormsApplication1
{
    class ClientManager
    {
        private const int RECEIVE_TIMEOUT = 10000;
        private BackgroundWorker clientThread;
        private StateObject stateClient;
        private AsyncManagerServer.StatusDelegate statusDelegate;
        private ManualResetEvent receiveDone;
        private Boolean syncEnd = false;
        private Boolean wellEnd = true;
        private List<FileChecksum> userChecksum;
        private List<FileChecksum> tempCheck;
        private SyncCommand cmd;
        private SyncSQLite mySQLite;
        private String serverDir;
        private int maxVersionNumber;

        public ClientManager(Socket sock, String workDir, int maxVers, AsyncManagerServer.StatusDelegate sd)
        {
            // Allocate resources
            stateClient = new StateObject();
            receiveDone = new ManualResetEvent(false);
            clientThread = new BackgroundWorker(); //esegue su thread separato
            mySQLite = new SyncSQLite();
            tempCheck = new List<FileChecksum>();
            // Init client info
            statusDelegate = sd;
            stateClient.workSocket = sock;
            maxVersionNumber = maxVers;
            serverDir = workDir;

            // Init thread
            clientThread.DoWork += new DoWorkEventHandler(doClient); //attach an event handler to the backgroundworker
            clientThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(doClientComplete);//attach the event that is generated when doClient returns
            clientThread.RunWorkerAsync();
        }

        public void WellStop()
        {
            // todo Cosa succede se sto sincronizzando? devo fare un restore?
            syncEnd = true;
            wellEnd = true;
        }

        public void badStop()
        {
            syncEnd = true;
            wellEnd = false;
        }

        public void ReceiveCommand(Socket client)
        {
            if (!SocketConnected(stateClient.workSocket))
            {
                badStop();
                receiveDone.Set();
                return;
            }

            // Begin receiving the data from the remote device.
            IAsyncResult iAR = client.BeginReceive(stateClient.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), null);
            bool success = iAR.AsyncWaitHandle.WaitOne(RECEIVE_TIMEOUT, true);

            if (!success)
            {
                statusDelegate("Timeout Expired", fSyncServer.LOG_WARNING);
                badStop();
                receiveDone.Set();
                return;
            }
        }


        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Read data from the remote device.
                if ((!syncEnd) && stateClient.workSocket.Connected == true)
                {
                    int bytesRead = stateClient.workSocket.EndReceive(ar);

                    if ((bytesRead > 0))
                    {
                        // There might be more data, so store the data received so far.
                        stateClient.sb.Append(Encoding.ASCII.GetString(stateClient.buffer, 0, bytesRead));
                    }
                    if (SyncCommand.searchJsonEnd(stateClient.sb.ToString()) == -1)
                    {
                        // Get the rest of the data.
                        stateClient.workSocket.BeginReceive(stateClient.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), null);
                    }
                    else
                    {
                        // All the data has arrived; put it in response.
                        if (stateClient.sb.Length > 1)
                        {
                            cmd = SyncCommand.convertFromString(stateClient.sb.ToString());
                            stateClient.sb.Clear();
                            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ACK));
                        }
                        // Signal that all bytes have been received.
                        receiveDone.Set();
                    }
                }
                else
                    receiveDone.Set();
            }
            catch (Exception e)
            {
                if (!syncEnd)
                {
                    statusDelegate("Exception: " + e.Message, fSyncServer.LOG_ERROR);
                    badStop();
                }
            }
        }

        public Boolean SocketConnected(Socket s)
        {
            //determina lo stato del socket
            bool part1 = s.Poll(1000, SelectMode.SelectRead); 
            bool part2 = (s.Available == 0); //se ha ricevuto qualcosa o meno
            if (part1 && part2)
                return false;
            else
                return true;
        }


        private void doClient(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!syncEnd)
                {
                    receiveDone.Reset();
                    // Receive the response from the remote device.
                    this.ReceiveCommand(stateClient.workSocket);
                    if (!syncEnd)
                    {
                        receiveDone.WaitOne();
                        if (doCommand())
                            statusDelegate("Slave Thread Done Command Successfully ", fSyncServer.LOG_INFO);
                        else
                            statusDelegate("Slave Thread Done Command with no Success", fSyncServer.LOG_ERROR);
                    }
                    else break;
                }
                if (!wellEnd)
                    statusDelegate("All NOT Well End Terminated", fSyncServer.LOG_ERROR);
                else
                    statusDelegate("All Well End Terminated", fSyncServer.LOG_INFO);
            }
            catch (Exception ex)
            {
                if (!syncEnd)
                {
                    statusDelegate("Exception: " + ex.Message, fSyncServer.LOG_ERROR);
                }
            }
        }

        public void doClientComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            // todo Cosa succede se sto sincronizzando? devo fare un restore?
            stateClient.workSocket.Close();
            AsyncManagerServer.DecreaseClient();
            statusDelegate("Server Stopped ", fSyncServer.LOG_INFO);
            mySQLite.closeConnection();
            if (tempCheck.Count > 0)
            {
                foreach (FileChecksum check in tempCheck)
                {
                    File.Delete(check.FileNameServer);
                    statusDelegate("Delete File: " + check.FileNameServer, fSyncServer.LOG_WARNING);
                }
                tempCheck.Clear();
            }
        }


        public Boolean doCommand()
        {
            if (cmd != null)
            {
                switch (cmd.Type)
                {
                    case SyncCommand.CommandSet.LOGIN:
                        statusDelegate("Command -> LOGIN", fSyncServer.LOG_INFO);
                        return LoginUser();
                    case SyncCommand.CommandSet.START:
                        statusDelegate(" Command -> START ", fSyncServer.LOG_INFO);
                        return StartSession();
                    /*case SyncCommand.CommandSet.RESTORE:
                        statusDelegate("Command -> RESTORE", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return RestoreVersion();*/
                    case SyncCommand.CommandSet.ENDSYNC:
                        statusDelegate("Command -> ENDSYNC", fSyncServer.LOG_INFO);
                        return EndSync();
                    case SyncCommand.CommandSet.NOSYNC:
                        statusDelegate("Command -> NOSYNC", fSyncServer.LOG_INFO);
                        return NoSync();
                    case SyncCommand.CommandSet.DEL:
                        statusDelegate("Command -> DEL", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return DeleteFile();
                    case SyncCommand.CommandSet.NEW:
                        statusDelegate("Command -> NEW", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return NewFile();
                    case SyncCommand.CommandSet.EDIT:
                        statusDelegate("Command -> EDIT", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return EditFile();
                    case SyncCommand.CommandSet.NEWUSER:
                        statusDelegate("Command -> NEWUSER", fSyncServer.LOG_INFO);
                        return NewUser();/*
                    case SyncCommand.CommandSet.GETVERSIONS:
                        statusDelegate("Command -> GETVERSIONS", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return GetVersions();
                    case SyncCommand.CommandSet.FILEVERSIONS:
                        statusDelegate("Command -> FILEVERSIONS", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return GetFileVersions();
                    case SyncCommand.CommandSet.GET:
                        statusDelegate("Command -> GET", fSyncServer.LOG_INFO);
                        if (stateClient.userID == -1)
                        {
                            statusDelegate("You must login before using this command", fSyncServer.LOG_ERROR);
                            return false;
                        }
                        return GetFile();*/
                    default:
                        statusDelegate("Received Wrong Command", fSyncServer.LOG_ERROR);
                        badStop();
                        return false;
                }

            }
            else
            {
                statusDelegate("Null Command Received", fSyncServer.LOG_ERROR);
                return false;
            }
        }

        public void SendCommand(Socket handler, SyncCommand command)
        {
            if (!syncEnd)
            {   // Convert the string data to byte data using ASCII encoding.

                byte[] byteData = Encoding.ASCII.GetBytes(command.convertToString());
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                // Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                if (!syncEnd)
                {
                    int bytesSent = stateClient.workSocket.EndSend(ar);
                }
            }
            catch (Exception e)
            {
                if (!syncEnd)
                {
                    statusDelegate("Exception: " + e.Message, fSyncServer.LOG_ERROR);
                    badStop();
                }
            }
        }

        //commands
        public Boolean LoginUser()
        {
            statusDelegate("[LoginUser] Get user data on DB", fSyncServer.LOG_INFO);
            Int64 userID = mySQLite.authenticateUser(cmd.Username, cmd.Password);
            if (userID >= 0)
            {
                statusDelegate("[LoginUser] User Credential Confermed", fSyncServer.LOG_INFO);
                stateClient.userID = userID;
                serverDir += "\\user" + stateClient.userID;
                stateClient.username = cmd.Username;
                stateClient.password = cmd.Password;
                statusDelegate("[LoginUser] Send Back Authorized Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                return true;
            }
            else
            {
                statusDelegate("[LoginUser] User Credential NOT Confirmed", fSyncServer.LOG_INFO);
                statusDelegate("[LoginUser] Send Back Unauthorized Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                return true;
            }
        }

        public Boolean NewUser()
        {
            if (cmd.Username == "" || cmd.Password == "")
            {
                statusDelegate("[NewUser] Username or password cannot be empty", fSyncServer.LOG_INFO);
                statusDelegate("[NewUser] Send Back Unauthorized Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                return true;
            }

            Int64 userID = mySQLite.newUser(cmd.Username, cmd.Password, cmd.Directory);
            if (userID == -1)
            {
                statusDelegate("[NewUser] Username in CONFLICT choose another one", fSyncServer.LOG_INFO);
                statusDelegate("[NewUser] Send Back Unauthorized Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                return true;
            }
            else
            {
                statusDelegate("[NewUser] User Added Succesfully", fSyncServer.LOG_INFO);
                stateClient.userID = userID;
                stateClient.username = cmd.Username;
                stateClient.password = cmd.Password;
                stateClient.directory = cmd.Directory;
                serverDir += "\\user" + stateClient.userID;
                stateClient.version = 0;
                statusDelegate("[NewUser] Send Back Authorized Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                return true;
            }
        }

        public Boolean StartSession()
        {
            Int64 userID = mySQLite.checkUserDirectory(stateClient.username, cmd.Directory); //Call DB Check Directory User
            if (userID == -1)
            {
                //TODO: FRA e Ro, vedere gestione cartelle differenti
                statusDelegate("[StartSession] User Directory Change NOT Authorized", fSyncServer.LOG_INFO);
                statusDelegate("[StartSession] Send Back Unauthorized Message because the user change the root directory for the connection", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.UNAUTHORIZED));
                return true;
            }
            else
            {
                stateClient.userID = userID;
                stateClient.directory = cmd.Directory;
                Int64 lastVers = 0;
                mySQLite.getUserMinMaxVersion(stateClient.userID, ref lastVers);
                stateClient.version = lastVers; //Call DB Get Last Version
                statusDelegate("[StartSession] User Directory Authorized, Start Send Check", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.AUTHORIZED));
                userChecksum = mySQLite.getUserFiles(stateClient.userID, stateClient.version, serverDir); //Call DB Get Users Files

                foreach (FileChecksum check in userChecksum)
                {
                    SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.CHECK, check.FileNameClient, check.Checksum.ToString()));
                    statusDelegate("[StartSession] Send check Message", fSyncServer.LOG_INFO);
                }
                tempCheck.Clear();
                statusDelegate("[StartSession] Send End check Message", fSyncServer.LOG_INFO);
                SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ENDCHECK));
                return true;
            }


        }

        public Boolean NewFile()
        {
            string fileNameDB = Utility.FilePathWithVers(cmd.FileName, stateClient.version + 1);
            ReceiveFile(serverDir + fileNameDB, cmd.FileSize);
            statusDelegate("[NewFile] Received New File correcty", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName, serverDir + fileNameDB, fileNameDB);
            tempCheck.Add(file);
            return true;
        }

        public void ReceiveFile(String fileName, Int64 fileLength)
        {
            byte[] buffer = new byte[1024];
            int rec = 0;

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            BinaryWriter bFile = new BinaryWriter(File.Open(fileName, FileMode.Create));

            // Receive data from the server
            while (fileLength > 0)
            {
                rec = stateClient.workSocket.Receive(buffer);
                fileLength -= rec;
                bFile.Write(buffer, 0, rec);
            }
            bFile.Close();
            SendCommand(stateClient.workSocket, new SyncCommand(SyncCommand.CommandSet.ACK));

        }

        public Boolean EditFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("[EditFile] File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            string fileNameDB = Utility.FilePathWithVers(cmd.FileName, stateClient.version + 1);
            ReceiveFile(serverDir + fileNameDB, cmd.FileSize);
            statusDelegate("[EditFile] Received File to Edit correcty", fSyncServer.LOG_INFO);
            FileChecksum file = new FileChecksum(cmd.FileName, serverDir + fileNameDB, fileNameDB);
            tempCheck.Add(file);
            return true;
        }

        public Boolean DeleteFile()
        {
            int index = userChecksum.FindIndex(x => x.FileNameClient == cmd.FileName);
            userChecksum.RemoveAt(index);
            statusDelegate("[DeleteFile] File Correctly Delete from the list of the files of the current Version", fSyncServer.LOG_INFO);
            return true;
        }

        public Boolean EndSync()
        {
            stateClient.version++;
            foreach (FileChecksum check in userChecksum)
            {
                tempCheck.Add(check);
            }
            userChecksum.Clear();
            mySQLite.setUserFiles(stateClient.userID, stateClient.version, tempCheck); // Call DB Update to new Version all the Files
            tempCheck.Clear();
            statusDelegate("[EndSync] DB Updated Correctly", fSyncServer.LOG_INFO);
            CancelVersion();
            WellStop();
            return true;
        }

        public Boolean CancelVersion()
        {

            Int64 maxVers = 0;
            Int64 minVers = mySQLite.getUserMinMaxVersion(stateClient.userID, ref maxVers);
            Int64 diff = maxVers - minVers;
            while (diff >= maxVersionNumber)
            {
                userChecksum = mySQLite.getUserFiles(stateClient.userID, minVers, serverDir); //Call DB Get Users Files;
                minVers++;
                tempCheck = mySQLite.getUserFiles(stateClient.userID, minVers, serverDir); //Call DB Get Users Files;
                foreach (FileChecksum check in userChecksum)
                {

                    int index = tempCheck.FindIndex(x => x.FileNameServer == check.FileNameServer);

                    if (index == -1)
                    {
                        File.Delete(check.FileNameServer);
                        statusDelegate("Deleted File Correctly:" + check.FileNameServer, fSyncServer.LOG_INFO);
                    }

                }
                mySQLite.deleteVersion(stateClient.userID, minVers - 1);
                diff--;

            }
            userChecksum.Clear();
            tempCheck.Clear();
            return true;
        }

        public Boolean NoSync()
        {
            WellStop();
            userChecksum.Clear();
            tempCheck.Clear();
            CancelVersion();
            return true;
        }


    }
}
