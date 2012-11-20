namespace System.Net.Sockets
{
    using Net;
    using Net.Sockets;
    using Threading;
    using Collections.Generic;
    using Text;

    /// <summary>
    /// Simple Threaded TCP Listener
    /// </summary>
    public class SyncListener
    {
        public TcpListener TcpListener { get; private set; }

        public List<TcpClient> ClientList { get; private set; }

        public volatile bool IsRunning = false;

        public Action<String> Action { get; set; }

        public int Sleep = 100;

        public Thread ThreadListen { get; set; }

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
                ThreadListen = new Thread(new ThreadStart(DoListening))
                                    {
                                        Name = "Listener Thread",
                                        Priority = ThreadPriority.AboveNormal
                                    };
                ThreadListen.Start();
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
                        tcpClient.Close();
                    }
                }
            }
        }

        // ------------------------

        void DoListening()
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
                    var threadClient = new Thread(new ParameterizedThreadStart(HandleClientConnect))
                                            {
                                                Name = "Client Thread " + (ClientList.Count),
                                                Priority = ThreadPriority.Normal,
                                            };
                    threadClient.Start(tcpClient);
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
                    var stream = tcpClient.GetStream();
                    if (stream.CanWrite)
                    {
                        var buffer = Encoding.ASCII.GetBytes(data);
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Flush();
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
                var stream = tcpClient.GetStream();
                if (stream.CanRead)
                {
                    var bufferSize = 256;
                    var buffer = new byte[bufferSize];
                    try
                    {
                        do
                        {
                            int noOfBytes = stream.Read(buffer, 0, bufferSize);
                            sbData.Append(Encoding.ASCII.GetString(buffer, 0, noOfBytes));
                        }
                        while (tcpClient.Connected && stream.DataAvailable);
                    }
                    catch (Exception)
                    { }
                }
            }
            return sbData.ToString();
        }

    }
}
