using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using System;

namespace WindowsFormsApplication1
{
    class AsyncManagerServer
    {
        private const int SOCKET_QUEUE_LENGTH = 100;
        public delegate void StatusDelegate(String s, int type);
        public delegate void NumberDelegate(int nclient);
        private StatusDelegate statusDelegate;
        private static NumberDelegate numberDelegate;
        private static int clientNumber = 0;
        private System.Collections.Generic.List<ClientManager> clients;
        private int localport;
        private IPAddress localAddr;
        private String defaultDir;
        private Thread listeningThread;
        private bool serverStopped;
        private int defaultMaxVers;
        private Socket listener;

        public AsyncManagerServer(StatusDelegate sd, NumberDelegate nd)
        {
            statusDelegate = sd;
            numberDelegate = nd;
            //instantiate list of clientManager
            clients = new System.Collections.Generic.List<ClientManager>();
            localAddr = IPAddress.Any;
        }

        
        public void startSync(int port, String workDir, int maxVers)
        {
            // Function Start Sync Button --> start server connection
            // Check if the directory is valid
            if (!Directory.Exists(workDir))
            {
                throw new System.Exception("Directory not exists");
            }
            defaultDir = workDir;
            if (defaultDir[defaultDir.Length - 1] == '\\')
            {
                defaultDir = defaultDir.Substring(0, defaultDir.Length - 1);
            }
            defaultMaxVers = maxVers;
            localport = port;
            // Server start
            serverStopped = false;
            listeningThread = new Thread(new ThreadStart(StartListening));
            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        public void StartListening()
        {
            /* establish new connection and start listening for connection*/
            try
            {
                // Establish the local endpoint for the socket.
                IPEndPoint localEndPoint = new IPEndPoint(localAddr, localport);
                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                listener.Bind(localEndPoint);
                listener.Listen(SOCKET_QUEUE_LENGTH);

                statusDelegate("Start Listening on Port: " + localport + "; Address: " + localAddr, fSyncServer.LOG_INFO);
                while (!serverStopped)
                {
                    // Start an synchronous socket to listen for connections.
                    Socket handler = listener.Accept();
                    ClientManager client = new ClientManager(handler, defaultDir, defaultMaxVers, statusDelegate);
                    AsyncManagerServer.IncreaseClient();
                    clients.Add(client);

                    statusDelegate("Connected and Created New Thred to Serve Client", fSyncServer.LOG_INFO);
                }
            }
            catch (Exception e)
            {
                statusDelegate("Connection Error Exception:" + e.ToString(), fSyncServer.LOG_INFO);
            }
            finally
            {
                // Close socket and clients
                if (listener.Connected) listener.Close();
                foreach (ClientManager client in clients)
                {
                    client.WellStop();
                }
            }
        }


        // Manage connected user count
        static public void IncreaseClient()
        {
            clientNumber++;
            numberDelegate(clientNumber);
        }

        static public void DecreaseClient()
        {
            clientNumber--;
            numberDelegate(clientNumber);
        }



    }
}
