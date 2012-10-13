namespace System.Net.Sockets
{
    using Net;
    using Net.Sockets;
    using Text;

    public class UdpSocket
    {
        public Socket Socket { get; private set; }

        public IPEndPoint LocalEP { get; private set; }

        public UdpSocket(IPAddress ipAddress, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            LocalEP = new IPEndPoint(ipAddress, port);
        }

        public void SendData(String data)
        {
            try
            {
                var buffer = Encoding.ASCII.GetBytes(data);
                var noOfBytes = Socket.SendTo(buffer, LocalEP);
            }
            catch { }
        }

        public String ReceiveData()
        {
            var sbData = new StringBuilder();

            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var remoteEP = LocalEP as EndPoint;
            try
            {
                //do
                {
                    int noOfBytes = Socket.ReceiveFrom(buffer, ref remoteEP);
                    sbData.Append(Encoding.ASCII.GetString(buffer, 0, noOfBytes));
                }
                //while (Socket.Available > 0);
            }
            catch { }
            return sbData.ToString();
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
