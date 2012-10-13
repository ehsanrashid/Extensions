namespace System.Net.Sockets
{
    using Net;
    using Net.Sockets;
    using Threading;
    using Collections.Generic;
    using Text;

    /// <summary>
    /// Simple Threaded TCP Server
    /// </summary>
    public class SyncListener
    {
        public TcpListener TcpListener { get; private set; }

        public List<TcpClient> ClientList { get; private set; }

        public volatile bool IsRunning = false;

        public Action<String> Action { get; set; }

        public int Sleep = 100;

        Thread thListen { get; set; }


        public SyncListener(int port)
        {

            TcpListener = new TcpListener(IPAddress.Any, port);
            ClientList = new List<TcpClient>(5);
        }


        public void Start()
        {
            try
            {
                // Listener Accept Clients Thread
                thListen = new Thread(new ThreadStart(StartListening))
                            {
                                Name = "Listener Thread",
                                Priority = ThreadPriority.AboveNormal
                            };
                thListen.Start();
            }
            catch (Exception)
            { }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                try
                {
                    TcpListener.Stop();
                    IsRunning = false;
                }
                catch (Exception)
                { }
            }

            lock (ClientList)
            {
                foreach (var tcpClient in ClientList)
                {
                    if (tcpClient.Connected)
                    {
                        tcpClient.Client.Close();
                        tcpClient.Close();
                    }
                }
            }
        }

        // ------------------------

        void StartListening()
        {
            TcpListener.Start();
            IsRunning = true;
            while (IsRunning)
            {
                try
                {
                    //blocks until a client has connected to the server
                    var tcpClient = TcpListener.AcceptTcpClient();
                    //create a thread to handle communication with connected client
                    var thClient = new Thread(new ParameterizedThreadStart(HandleClientConnect))
                                    {
                                        Name = "Client Thread " + (ClientList.Count),
                                        Priority = ThreadPriority.Normal,
                                    };
                    thClient.Start(tcpClient);
                }
                catch { }
            }
        }

        void HandleClientConnect(Object obj)
        {
            var tcpClient = obj as TcpClient;
            if (default(TcpClient) != tcpClient)
            {
                lock (ClientList)
                {
                    ClientList.Add(tcpClient);
                }

                while (IsRunning && tcpClient.Connected)
                {
                    var data = ReceiveFromClient(tcpClient);
                    if (data.IsNotNullOrEmpty())
                    {
                        if (Action != default(Action<String>))
                        {
                            Action(data);
                        }
                    }
                    Thread.Sleep(Sleep);
                }

                if (tcpClient.Connected)
                {
                    tcpClient.Client.Close();
                    tcpClient.Close();
                }

                lock (ClientList)
                {
                    ClientList.Remove(tcpClient);
                }
            }
        }

        void SendToAll(String data)
        {
            lock (ClientList)
            {
                foreach (var tcpClient in ClientList)
                {
                    SendToClient(tcpClient, data);
                }
            }
        }

        void SendToClient(TcpClient tcpClient, String data)
        {
            if (tcpClient != default(TcpClient) && tcpClient.Connected)
            {
                try
                {
                    var nwStream = tcpClient.GetStream();
                    if (nwStream.CanWrite)
                    {
                        var buffer = Encoding.ASCII.GetBytes(data);
                        nwStream.Write(buffer, 0, buffer.Length);
                        nwStream.Flush();
                    }
                }
                catch (Exception)
                { }
            }
        }

        String ReceiveFromClient(TcpClient tcpClient)
        {
            var sbData = new StringBuilder();
            if (tcpClient != default(TcpClient) && tcpClient.Connected)
            {
                var nwStream = tcpClient.GetStream();
                if (nwStream.CanRead)
                {
                    var bufferSize = 256;
                    var buffer = new byte[bufferSize];
                    try
                    {
                        do
                        {
                            int noOfBytes = nwStream.Read(buffer, 0, bufferSize);
                            sbData.Append(Encoding.ASCII.GetString(buffer, 0, noOfBytes));
                        }
                        while (tcpClient.Connected && nwStream.DataAvailable);
                    }
                    catch (Exception)
                    { }
                }
            }
            return sbData.ToString();
        }
    }
}
