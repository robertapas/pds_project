﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace clientWpf
{
    public class SyncManager
    {
        public delegate void StatusDelegate(String s, bool fatalError = false);
        public delegate void StatusBarDelegate(int percentage);

        private String address, username, password, syncDirectory;
        private int port;
        private Thread syncThread;
        private List<FileChecksum> serverFileChecksum, clientFileChecksum;
        private bool thread_stopped = false, someChanges = false;
        private StatusDelegate statusDelegate;
        private StatusBarDelegate statusBarDelegate;
        private Socket tcpClient;
        private String receivedBuffer = "";
        private Mutex connectionMutex;
        private int sync_sleeping_time = 5000;
        private System.Timers.Timer syncSleepTimer;
        private AutoResetEvent doSyncEvent;
        private Boolean syncEnd = true;

        public SyncManager()
        {
            serverFileChecksum = new List<FileChecksum>();
            clientFileChecksum = new List<FileChecksum>();
            connectionMutex = new Mutex();
            doSyncEvent = new AutoResetEvent(false); //come una wait apetta che un evento si verifica, se inizializzato a falso è in stato non segnalato
        }

        public void setStatusDelegate(StatusDelegate sd, StatusBarDelegate sbd)
        {
            this.statusDelegate = sd;
            this.statusBarDelegate = sbd;
        }


        public async Task<bool> login(String address, int port, String username, String password, String directory = "", bool register = false)
        {
            bool authorized = false;
            Exception ex = null;
            await Task.Run(() =>
            {
                try
                {
                    this.address = address;
                    this.port = port;
                    this.username = username;
                    this.password = password;
                    statusBarDelegate(0);
                    serverConnect();
                    statusBarDelegate(50);
                    if (register)
                    {
                        this.sendCommand(new SyncCommand(SyncCommand.CommandSet.NEWUSER, this.username, this.password, directory));
                    }
                    else
                    {
                        this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, this.username, this.password));
                    }
                    authorized = (this.receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED);
                    tcpClient.Close();
                    statusBarDelegate(100);
                }
                catch (SocketException se)
                {
                    ex = se;
                }
                catch (Exception exx)
                {
                    ex = exx;
                }
            });
            if (ex != null) throw ex;
            return authorized;
        }


        public void stopSync()
        {

            this.thread_stopped = true;
            // Release the socket.


            if (tcpClient.Connected)
            {
                tcpClient.Shutdown(SocketShutdown.Both);
                tcpClient.Close();
            }
            if (syncThread != null && syncThread.IsAlive)
            {
                syncThread.Abort(); 
            }
        }

        private void sendCommand(SyncCommand command)
        {
            int bytesSent;
            // Get the command string
            String sCommand = command.convertToString(); //serialize to send

            // Send the data through the socket
            while (sCommand.Length > 0)
            {
                //codifica in byte e invia 
                bytesSent = tcpClient.Send(Encoding.ASCII.GetBytes(sCommand));
                // cat the message part already sent
                sCommand = sCommand.Substring(bytesSent);
            }

            if (receiveCommand().Type != SyncCommand.CommandSet.ACK)
            {
                statusDelegate("Protocol error", true);
            }
        }

        private SyncCommand receiveCommand()
        {
            byte[] data = new byte[1024];
            int dataRec, jsonEnd;
            SyncCommand sc;
            while ((jsonEnd = SyncCommand.searchJsonEnd(receivedBuffer)) == -1)
            {
                // Receive data from the server

                if (!SocketConnected(tcpClient))
                {
                    if (syncEnd == false)
                    {
                        statusDelegate("Server is not responding. Stop syncing.");
                        stopSync();
                        return null;
                    }
                    statusDelegate("Server is not responding");
                }
                dataRec = tcpClient.Receive(data);
                receivedBuffer += Encoding.ASCII.GetString(data, 0, dataRec);

            }
            sc = SyncCommand.convertFromString(receivedBuffer.Substring(0, jsonEnd + 1));
            receivedBuffer = receivedBuffer.Substring(jsonEnd + 1);
            return sc;
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


        private void serverConnect()
        {
            IPAddress ipAddress;
            // Generate the remote endpoint
            statusBarDelegate(5);
            if (Regex.IsMatch(address, "^\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}$"))
            {
                //is ipv4 address
                String[] parts = address.Split('.');
                ipAddress = new IPAddress(new byte[] { Byte.Parse(parts[0]), Byte.Parse(parts[1]), Byte.Parse(parts[2]), Byte.Parse(parts[3]) });
            }
            else
            {
                statusDelegate("Request address form DNS");
                IPHostEntry ipHostInfo = Dns.GetHostEntry(address);
                ipAddress = ipHostInfo.AddressList[0];
            }
            statusBarDelegate(8);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket
            tcpClient = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Connect to the remote endpoint
            statusDelegate("Starting connection...");

            // connect with timeout
            IAsyncResult result = tcpClient.BeginConnect(remoteEP, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);

            if (!success)
            {
                tcpClient.Close();
                throw new ApplicationException("Connection timeout.");
            }

            statusDelegate("Connected to: " + tcpClient.RemoteEndPoint.ToString());
            statusBarDelegate(10);
        }

        public void startSync(String address, int port, String directory, int sleeping_time)
        {
            // Check if the directory is valid
            statusBarDelegate(0);
            if (!Directory.Exists(directory))
            {
                throw new Exception("ERROR: Directory not exists");
            }
            this.syncDirectory = directory;
            if (directory[directory.Length - 1] == '\\')
            {
                directory = directory.Substring(0, directory.Length - 1);
            }
            this.receivedBuffer = "";
            this.address = address;
            this.port = port;
            this.sync_sleeping_time = sleeping_time;
            syncSleepTimer = new System.Timers.Timer(this.sync_sleeping_time);
            syncSleepTimer.Elapsed += new System.Timers.ElapsedEventHandler((object o, System.Timers.ElapsedEventArgs e) => { doSyncEvent.Set(); });
            syncSleepTimer.AutoReset = false;

            // Start the sync thread
            this.thread_stopped = false;
            this.syncThread = new Thread(new ThreadStart(this.doSync));
            this.syncThread.IsBackground = true;
            this.syncThread.Start();
        }

        private void doSync()
        {
            try
            {
                // Do syncking
                syncEnd = false;
                while (!thread_stopped)
                {
                    connectionMutex.WaitOne();
                    // connect
                    statusBarDelegate(0);
                    serverConnect();
                    statusDelegate("Syncing...");
                    // login
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
                    if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Login failed", true);
                        return;
                    }
                    statusBarDelegate(15);
                    // start
                    sendCommand(new SyncCommand(SyncCommand.CommandSet.START, syncDirectory));
                    if (receiveCommand().Type != SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Wrong directory", true);
                        return;
                    }
                    // get check list
                    statusBarDelegate(20);
                    serverFileChecksum = getServerCheckList();
                    // scan client files
                    statusBarDelegate(25);
                    someChanges = false;
                    scanForClientChanges(syncDirectory);
                    // send delete files
                    statusBarDelegate(80);
                    scanForDeletedFiles();
                    // commit changes
                    statusBarDelegate(90);
                    commitChangesToServer(someChanges);
                    syncEnd = true;
                    statusBarDelegate(100);
                    // close connection
                    tcpClient.Close();
                    statusDelegate("Idle");
                    connectionMutex.ReleaseMutex();

                    // Setup timer
                    syncSleepTimer.Start();
                    doSyncEvent.Reset();
                    doSyncEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                statusDelegate(ex.Message, true);
            }
            finally
            {
                try
                {
                    connectionMutex.ReleaseMutex(); 
                }
                catch (Exception e)
                {
                    //statusDelegate(e.Message, true);
                }
            }
        }

        private List<FileChecksum> getServerCheckList()
        {
            SyncCommand sc;
            List<FileChecksum> serverCheckList = new List<FileChecksum>();

            while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
            {
                if (sc.Type != SyncCommand.CommandSet.CHECK) throw new Exception("Check list receive error");
                serverCheckList.Add(new FileChecksum(sc.FileName, Encoding.ASCII.GetBytes(sc.Checksum)));
            }

            return serverCheckList;
        }

        private void scanForClientChanges(String dir)
        {
            // Get directory file list
            string[] fileList = Directory.GetFiles(dir);

            // Scan for changes
            foreach (string filePath in fileList)
            {
                FileChecksum currentFile = new FileChecksum(filePath, syncDirectory);
                // Search the file in the server list
                int pos = serverFileChecksum.FindIndex(x => (x.BaseFileName == currentFile.BaseFileName));

                if (pos < 0)
                {
                    // create a new file on the server
                    someChanges = true;
                    FileInfo fi = new FileInfo(currentFile.FileName);
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.NEW, currentFile.BaseFileName, fi.Length.ToString()));
                    this.sendFile(currentFile.FileName);
                }
                else
                {
                    // the file is also on the server
                    if (currentFile.Checksum != serverFileChecksum[pos].Checksum)
                    {
                        // on the server there is a different version of the file
                        someChanges = true;
                        FileInfo fi = new FileInfo(currentFile.FileName);
                        this.sendCommand(new SyncCommand(SyncCommand.CommandSet.EDIT, currentFile.BaseFileName, fi.Length.ToString()));
                        this.sendFile(currentFile.FileName);
                    }
                    serverFileChecksum.RemoveAt(pos);
                }

                clientFileChecksum.Add(currentFile);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryList = Directory.GetDirectories(dir);
            foreach (string subdirectoryPath in subdirectoryList)
            {
                this.scanForClientChanges(subdirectoryPath);
            }
        }

        private void sendFile(String path)
        {
            FileInfo fi = new FileInfo(path);
            byte[] buffer;
            Int64 fileLength = fi.Length, byteSent = 0;
            statusBarDelegate(0);
            BinaryReader bFile = new BinaryReader(File.Open(path, FileMode.Open));
            while (byteSent < fileLength)
            {
                if (fileLength - byteSent < 100)
                {
                    buffer = bFile.ReadBytes((Int32)(fileLength - byteSent));
                    byteSent = fileLength;
                }
                else
                {
                    buffer = bFile.ReadBytes(100);
                    byteSent += 100;
                }
                tcpClient.Send(buffer);
                statusBarDelegate((Int32)(byteSent * 90 / fileLength));
            }

            bFile.Close();
            //tcpClient.SendFile(path);
            statusBarDelegate(90);
            if (receiveCommand().Type != SyncCommand.CommandSet.ACK)
            {
                statusDelegate("Error during file trasmission", true);
            }
            statusBarDelegate(100);
        }

        private void scanForDeletedFiles()
        {
            foreach (FileChecksum currentFile in serverFileChecksum)
            {
                someChanges = true;
                sendCommand(new SyncCommand(SyncCommand.CommandSet.DEL, currentFile.BaseFileName));
            }
        }

        private void commitChangesToServer(bool changes)
        {
            if (changes)
                sendCommand(new SyncCommand(SyncCommand.CommandSet.ENDSYNC));
            else
                sendCommand(new SyncCommand(SyncCommand.CommandSet.NOSYNC));
            serverFileChecksum = clientFileChecksum;
            clientFileChecksum.Clear();
        }

        public void forceSync()
        {
            doSyncEvent.Set();
        }

        public async Task<List<Version>> getVersions()
        {
            List<Version> versions = new List<Version>();
            Exception ex = null;

            await Task.Run(() => //per non bloccare ui thread
            {
                Version version = null;
                Int64 versionNum = 0;
                SyncCommand sc;
                try
                {
                    connectionMutex.WaitOne();
                    serverConnect();
                    // login
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
                    if (receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Retrieve version list...");
                        sendCommand(new SyncCommand(SyncCommand.CommandSet.GETVERSIONS));
                        while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
                        {
                            switch (sc.Type)
                            {
                                case SyncCommand.CommandSet.VERSION:
                                    version = new Version(sc.Version, sc.Timestamp);
                                    versions.Add(version);
                                    versionNum = sc.Version;
                                    break;
                                case SyncCommand.CommandSet.CHECKVERSION:
                                    version.append(new VersionFile(sc.FileName, sc.Operation, versionNum));
                                    break;
                                default:
                                    throw new Exception("Version receive error");
                            }
                        }
                        statusBarDelegate(100);
                        statusDelegate("Versions retrieved");
                    }
                    else
                    {
                        statusDelegate("Login fail");
                    }

                }
                catch (Exception exx)
                {
                    ex = exx;
                }
                finally
                {
                    tcpClient.Close();
                    connectionMutex.ReleaseMutex();
                }
            });
            if (ex != null) throw ex;
            return versions;
        }

        public async Task restoreVersion(Int64 versionNum)
        {
            Exception ex = null;
            await Task.Run(() =>
            {
                SyncCommand sc;
                try
                {
                    connectionMutex.WaitOne();
                    serverConnect();
                    // login
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
                    if (receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Start restore...");
                        sendCommand(new SyncCommand(SyncCommand.CommandSet.RESTORE, versionNum.ToString()));
                        string tempDir = System.IO.Path.GetTempPath() + "syncClient";
                        while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDRESTORE)
                        {
                            if (sc.Type != SyncCommand.CommandSet.FILE) throw new Exception("Protocol error");
                            this.getFile(tempDir + sc.FileName, sc.FileSize);
                        }
                        // commit changes
                        if (Directory.Exists(tempDir) && Directory.EnumerateFileSystemEntries(tempDir).Any()) // if tempDir is not empty
                        {
                            Directory.Delete(syncDirectory, true);
                            this.moveFiles(tempDir, syncDirectory);
                            Directory.Delete(tempDir, true);
                        }
                        else
                        {
                            throw new Exception("You cannot restore an empty version");
                        }
                        statusDelegate("Restore done");
                    }
                    else
                    {
                        statusDelegate("Login fail");
                    }
                }
                catch (Exception exx)
                {
                    ex = exx;
                }
                finally
                {
                    tcpClient.Close();
                    connectionMutex.ReleaseMutex();
                }
            });
            if (ex != null) throw ex;
        }

        private void getFile(String fileName, int fileLength)
        {
            byte[] buffer = new byte[1024];
            int rec = 0, byteSent = 0;
            statusBarDelegate(0);
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            BinaryWriter bFile = new BinaryWriter(File.Open(fileName, FileMode.Create));

            // Check input buffer of commands
            if (receivedBuffer.Length > 0)
            {
                // there are some data
                if (receivedBuffer.Length <= fileLength)
                {
                    bFile.Write(Encoding.ASCII.GetBytes(receivedBuffer), 0, receivedBuffer.Length);
                    byteSent = receivedBuffer.Length;
                    receivedBuffer = "";
                }
                else
                {
                    bFile.Write(Encoding.ASCII.GetBytes(receivedBuffer.Substring(0, fileLength)), 0, fileLength);
                    receivedBuffer = receivedBuffer.Substring(0, fileLength);
                    byteSent = fileLength;
                }
            }

            // Receive data from the server
            statusBarDelegate((Int32)(byteSent * 90 / fileLength));
            while (byteSent < fileLength)
            {
                rec = tcpClient.Receive(buffer);
                bFile.Write(buffer, 0, rec);
                byteSent += rec;
                statusBarDelegate((Int32)(byteSent * 90 / fileLength));
            }
            bFile.Close();
            statusBarDelegate(90);
            this.sendCommand(new SyncCommand(SyncCommand.CommandSet.ACK));
            statusBarDelegate(100);
        }

        public async Task<List<VersionFile>> getFileVersions(string filename)
        {
            List<VersionFile> versions = new List<VersionFile>();
            Exception ex = null;
            await Task.Run(() =>
            {
                SyncCommand sc;
                try
                {
                    connectionMutex.WaitOne();
                    serverConnect();
                    // login
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
                    if (receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Retrieve file version list...");
                        sendCommand(new SyncCommand(SyncCommand.CommandSet.FILEVERSIONS, filename));
                        while ((sc = this.receiveCommand()).Type != SyncCommand.CommandSet.ENDCHECK)
                        {
                            switch (sc.Type)
                            {
                                case SyncCommand.CommandSet.CHECKVERSION:
                                    versions.Add(new VersionFile(sc.FileName, sc.Operation, sc.Version, sc.Timestamp));
                                    break;
                                default:
                                    throw new Exception("Version receive error");
                            }
                        }
                        statusDelegate("Versions retrieved");
                    }
                    else
                    {
                        statusDelegate("Login fail");
                    }

                }
                catch (Exception exx)
                {
                    ex = exx;
                }
                finally
                {
                    tcpClient.Close();
                    connectionMutex.ReleaseMutex();
                }
            });
            if (ex != null) throw ex;
            return versions;
        }

        private void moveFiles(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            string[] fileList = Directory.GetFiles(source);

            // Scan for changes
            foreach (string sourceFile in fileList)
            {
                File.Copy(sourceFile, destination + "\\" + Path.GetFileName(sourceFile), true);

            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryList = Directory.GetDirectories(source);
            foreach (string subdirectoryPath in subdirectoryList)
            {
                this.moveFiles(subdirectoryPath, destination + subdirectoryPath.Substring(source.Length));
            }
        }

        public async Task restoreFileVersion(string selectedFileName, Int64 selectedVersion)
        {
            Exception ex = null;
            await Task.Run(() =>
            {
                SyncCommand sc;
                try
                {
                    connectionMutex.WaitOne();
                    serverConnect();
                    // login
                    this.sendCommand(new SyncCommand(SyncCommand.CommandSet.LOGIN, username, password));
                    if (receiveCommand().Type == SyncCommand.CommandSet.AUTHORIZED)
                    {
                        statusDelegate("Start file restore...");
                        sendCommand(new SyncCommand(SyncCommand.CommandSet.GET, selectedFileName, selectedVersion.ToString()));
                        string tempDir = System.IO.Path.GetTempPath() + "syncClient";
                        sc = this.receiveCommand();
                        switch (sc.Type)
                        {
                            case SyncCommand.CommandSet.FILE:
                                this.getFile(tempDir + sc.FileName, sc.FileSize);
                                // commit changes
                                this.moveFiles(tempDir, syncDirectory);
                                Directory.Delete(tempDir, true);

                                statusDelegate("Restore done");
                                break;
                            case SyncCommand.CommandSet.ENDRESTORE:
                                throw new Exception("You cannot restore the current file version");
                            default:
                                throw new Exception("Protocol error");
                        }

                    }
                    else
                    {
                        statusDelegate("Login fail");
                    }
                }
                catch (Exception exx)
                {
                    ex = exx;
                }
                finally
                {
                    tcpClient.Close();
                    connectionMutex.ReleaseMutex();
                }
            });
            if (ex != null) throw ex;
        }

    }



    }
