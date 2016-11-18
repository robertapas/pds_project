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


    }
}
