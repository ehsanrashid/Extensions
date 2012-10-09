namespace System.Net.Sockets
{
    using Runtime.InteropServices;

    public static class TcpClientExtension
    {
        /// <summary>
        /// Using IOControl code to configue socket KeepAliveValues for line disconnection detection(because default is toooo slow) 
        /// </summary>
        /// <param name="tcpc">TcpClient</param>
        /// <param name="KeepAliveTime">The keep alive time. (ms)</param>
        /// <param name="KeepAliveInterval">The keep alive interval. (ms)</param>
        public static void SetSocketKeepAliveValues(this TcpClient tcpc, int KeepAliveTime, int KeepAliveInterval)
        {
            //KeepAliveTime: default value is 2hr
            //KeepAliveInterval: default value is 1s and Detect 5 times

            uint dummy = 0; //lenth = 4
            var inOptionValues = new byte[Marshal.SizeOf(dummy)*3];
                //size = lenth * 3 = 12
            var OnOff = true;

            BitConverter.GetBytes((uint) (OnOff ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint) KeepAliveTime).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint) KeepAliveInterval).CopyTo(inOptionValues, Marshal.SizeOf(dummy)*2);
            // of course there are other ways to marshal up this byte array, this is just one way
            // call WSAIoctl via IOControl

            // .net 1.1 type
            //int SIO_KEEPALIVE_VALS = -1744830460; //(or 0x98000004)
            //socket.IOControl(SIO_KEEPALIVE_VALS, inOptionValues, null); 

            // .net 3.5 type
            tcpc.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }
}