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
        private Boolean wellEnd = false;
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


        public void SendCommand(Socket handler, SyncCommand command)
        {
            if (!syncEnd)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(command.convertToString());
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
        }


    }
}
