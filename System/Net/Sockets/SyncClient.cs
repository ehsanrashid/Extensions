namespace System.Net.Sockets
{
    using Net;
    using Net.Sockets;
    using Text;
    using Threading;
    using Diagnostics;

    /// <summary>
    /// Simple TCP Client
    /// </summary>
    public class SyncClient
    {
        public TcpClient TcpClient { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public Action<String> Action { get; set; }

        public int Sleep = 300;

        volatile bool _isRunning = false;
        volatile bool _isSleeping = false;


        public SyncClient()
        {
            var hostName = Dns.GetHostName();
            var localAdd = Dns.GetHostEntry(hostName).AddressList[0];
            var localEP = new IPEndPoint(localAdd, 0);
            TcpClient = new TcpClient(); //new TcpClient(localEP);
        }

        public void Connect(IPEndPoint remoteEP)
        {
            try
            {
                EndPoint = remoteEP;

                var threadConnect = new Thread(new ThreadStart(DoConnection))
                                        {
                                            Name = "Connection Thread",
                                            Priority = ThreadPriority.Normal,
                                        };
                
                threadConnect.Start();
            }
            catch (Exception) { }
        }

        public void Connect(IPAddress remoteIP, int port)
        {
            Connect(new IPEndPoint(remoteIP, port));
        }

        public void Connect(String hostName, int port)
        {
            var hostAddresses = Dns.GetHostAddresses(hostName);
            if (hostAddresses.Length > 0)
            {
                Connect(hostAddresses[0], port);    
            }
        }

        public void Disconnect()
        {
            if (TcpClient.Connected)
            {
                //using (var stream = TcpClient.GetStream())
                //{
                //    if (default(NetworkStream) != stream && (stream.CanRead || stream.CanWrite))
                //        stream.Close();
                //}
                TcpClient.Close();
                _isRunning = false;
            }
        }

        public void Send(String data)
        {
            try
            {
                if (!TcpClient.Connected)
                {
                    _isSleeping = false;
                    Thread.Sleep(1000);
                }

                if (TcpClient.Connected)
                {
                    using (var stream = TcpClient.GetStream())
                    {
                        if (default(NetworkStream) != stream && stream.CanWrite)
                        {
                            var buffer = Encoding.ASCII.GetBytes(data);
                            stream.Write(buffer, 0, buffer.Length);
                            stream.Flush();
                        }
                    }
                }
            }
            catch { }
        }

        public String Receive()
        {
            var sbData = new StringBuilder();
            if (TcpClient.Connected)
            {
                using (var stream = TcpClient.GetStream())
                {
                    if (stream != default(NetworkStream) && stream.CanRead)
                    {
                        var bufferSize = 256;
                        var buffer = new byte[bufferSize];
                        try
                        {
                            var noOfBytes = stream.Read(buffer, 0, bufferSize);
                            sbData.Append(Encoding.ASCII.GetString(buffer, 0, noOfBytes));
                        }
                        catch { }
                    }
                }
            }
            return sbData.ToString();
        }

        // -------------------

        void DoConnection()
        {
            _isRunning = true;

            var tries = 0;
            do
            {
                ++tries;
                try
                {
                    TcpClient = new TcpClient();
                    TcpClient.Connect(EndPoint);

                    if (TcpClient.Connected)
                    {
                        DoReceiving();

                        tries = 0;
                    }
                    else
                    {
                        _isSleeping = true;

                        Action<int, int> fnSleep = delegate(int timeOut, int timeStep)
                        {
                            var stopwatch = Stopwatch.StartNew();
                            while (_isSleeping && stopwatch.ElapsedMilliseconds < timeOut)
                            {
                                Thread.Sleep(timeStep);
                            }
                        };

                        fnSleep((1000 * tries + 2000) * tries + 2000, 100);
                    }
                }
                catch { }
            }
            while (_isRunning);
        }

        void DoReceiving()
        {
            while (TcpClient.Connected)
            {
                try
                {
                    var data = Receive();
                    if (data.IsNotNullOrEmpty())
                    {
                        if (default(Action<String>) != Action)
                        {
                            Action(data);
                        }
                    }
                    Thread.Sleep(Sleep);
                }
                catch { }
            }
        }

    }
}
