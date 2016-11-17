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
            clientThread = new BackgroundWorker();
            mySQLite = new SyncSQLite();
            tempCheck = new List<FileChecksum>();
            // Init client info
            statusDelegate = sd;
            stateClient.workSocket = sock;
            maxVersionNumber = maxVers;
            serverDir = workDir;

            // Init thread
            clientThread.DoWork += new DoWorkEventHandler(doClient);
            clientThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(doClientComplete);
            clientThread.RunWorkerAsync();
        }

        public void WellStop()
        {
            // todo Cosa succede se sto sincronizzando? devo fare un restore?
            syncEnd = true;
            wellEnd = true;
        }

    }
}
