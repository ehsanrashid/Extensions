namespace System.Net.Sockets
{
    //using Reactive;
    //using Reactive.Linq;

    public static class SocketExtension
    {
        public static bool IsConnected(this Socket socket)
        {
            var canRead = socket.Poll(1000, SelectMode.SelectRead);
            var isAvailable = (socket.Available == 0);
            return canRead & isAvailable;
        }

        public static IPEndPoint RemoteEndPoint(this Socket socket)
        {
            return socket.RemoteEndPoint as IPEndPoint;
        }

        /*
        public static IObservable<Unit> WhenConnected(this Socket socket, IPAddress address, int port)
        {
            return Observable.FromAsyncPattern<IPAddress, int>(socket.BeginConnect, socket.EndConnect)(address, port);
        }


        public static IObservable<T> GetSocketData<T>(this Socket socket,
                                                      int sizeToRead, Func<byte[], T> valueExtractor)
        {
            return Observable.Create<T>(
                (observer) =>
                {
                    var readSize = Observable.FromAsyncPattern<byte[], int, int, SocketFlags, int>(
                            socket.BeginReceive,
                            socket.EndReceive);
                    var buffer = new byte[sizeToRead];
                    return readSize(buffer, 0, sizeToRead, SocketFlags.None)
                        .Subscribe(
                            (x) => observer.OnNext(valueExtractor(buffer)),
                            observer.OnError,
                            observer.OnCompleted);
                });
        }

        public static IObservable<int> GetMessageSize(this Socket socket)
        {
            return socket.GetSocketData(4, buf => BitConverter.ToInt32(buf, 0));
        }

        public static IObservable<String> GetMessageBody(this Socket socket,
                                                         int messageSize)
        {
            return socket.GetSocketData(messageSize, buf =>
                                                     Encoding.UTF8.GetString(buf, 0, messageSize));
        }

        public static IObservable<String> GetMessage(this Socket socket)
        {
            return socket.GetMessageSize()
                    .SelectMany(
                        (size) => Observable.If(
                            () => 0 != size,
                            socket.GetMessageBody(size),
                            Observable.Return<String>(null)));
        }


        public static IObservable<String> GetMessagesFromConnected(this Socket socket)
        {
            return socket
                .GetMessage()
                .Repeat()
                .TakeWhile(msg => msg.IsNotNullOrEmpty());
        }

        public static IObservable<String> GetMessages(this Socket socket, IPAddress addr, int port)
        {
            return Observable.Defer(
                () =>
                {
                    var whenConnect = Observable.FromAsyncPattern<IPAddress, int>(socket.BeginConnect, socket.EndConnect);
                    return whenConnect(addr, port).SelectMany(
                        (s) => socket.GetMessagesFromConnected().Finally(socket.Close));
                });
        }


        public static IObservable<byte[]> WhenDataReceived(this Socket socket, SocketFlags flags = SocketFlags.None)
        {
            return Observable.Create<byte[]>(
                observer =>
                {


                    var whenDataReceived = Observable.FromAsyncPattern<byte[], int, int, SocketFlags, int>(
                        socket.BeginReceive,
                        socket.EndReceive);

                    byte[] buffer = new byte[1024];

                    return Observable.While(() => socket.Connected,
                        Observable.Defer(() => whenDataReceived(buffer, 0, buffer.Length, flags)))
                        .Select(read => buffer.Take(read).ToArray())
                        .Subscribe(
                            observer.OnNext,
                            ex =>
                            {
                                var socketError = ex as SocketException;

                                if (socketError != null && socketError.SocketErrorCode == SocketError.Shutdown)
                                    observer.OnCompleted();
                                else
                                    observer.OnError(ex);
                            },
                            observer.OnCompleted);
                });
        }
        */

        /*
        public static IObservable<byte[]> WhenDataReceived(this Socket socket, int byteCount, SocketFlags flags = SocketFlags.None)
        {
            return Observable.Create<byte[]>(
                observer =>
                {
                    var whenDataReceived = Observable.FromAsyncPattern<byte[], int, int, SocketFlags, int>(
                        socket.BeginReceive,
                        socket.EndReceive);

                    byte[] buffer = new byte[byteCount];
                    int remainder = byteCount;

                    return Observable.While(() => remainder > 0,
                        Observable.Defer(() => whenDataReceived(buffer, buffer.Length - remainder, remainder, flags)))
                        .Prune(whenCompleted => whenCompleted.Select(_ => buffer))
                        .Subscribe(
                            observer.OnNext,
                            ex =>
                            {
                                var socketError = ex as SocketException;

                                if (socketError != null && socketError.SocketErrorCode == SocketError.Shutdown)
                                    observer.OnCompleted();
                                else
                                    observer.OnError(ex);
                            },
                            observer.OnCompleted);
                });
        }
        */


    }
}