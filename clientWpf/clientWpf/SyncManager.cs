using System;
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
                syncThread.Abort(); // TODO evitare di usare Abort
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
                dataRec = tcpClient.Receive(data);
                receivedBuffer += Encoding.ASCII.GetString(data, 0, dataRec);
            }
            sc = SyncCommand.convertFromString(receivedBuffer.Substring(0, jsonEnd + 1));
            receivedBuffer = receivedBuffer.Substring(jsonEnd + 1);
            return sc;
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

    }

}
