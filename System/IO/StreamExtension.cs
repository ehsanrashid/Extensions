namespace System.IO
{
    //using Reactive.Subjects;
    using Text;

    /// <summary>
    ///   Extension methods any kind of streams
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        ///   Opens a StreamReader using the specified encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <returns> The stream reader </returns>
        public static StreamReader GetReader(this Stream stream, Encoding encoding)
        {
            if (!stream.CanRead) throw new InvalidOperationException("Stream does not support reading.");
            return new StreamReader(stream, encoding ?? Encoding.Default);
        }

        /// <summary>
        ///   Opens a StreamReader using the default encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The stream reader </returns>
        public static StreamReader GetReader(this Stream stream) { return stream.GetReader(null); }

        /// <summary>
        ///   Opens a StreamWriter using the specified encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <returns> The stream writer </returns>
        public static StreamWriter GetWriter(this Stream stream, Encoding encoding)
        {
            if (!stream.CanWrite) throw new InvalidOperationException("Stream does not support writing.");
            return new StreamWriter(stream, encoding ?? Encoding.Default);
        }

        /// <summary>
        ///   Opens a StreamWriter using the default encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The stream writer </returns>
        public static StreamWriter GetWriter(this Stream stream) { return stream.GetWriter(null); }

        /// <summary>
        ///   Reads all text from the stream using a specified encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="encoding"> The encoding. </param>
        /// <returns> The result string. </returns>
        public static string ReadToEnd(this Stream stream, Encoding encoding)
        {
            if (!stream.CanRead) throw new InvalidOperationException("Stream does not support reading.");
            using (TextReader reader = stream.GetReader(encoding)) return reader.ReadToEnd();
        }

        /// <summary>
        ///   Reads all text from the stream using the default encoding.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The result string. </returns>
        public static string ReadToEnd(this Stream stream) { return stream.ReadToEnd(null); }

        /// <summary>
        ///   Sets the stream cursor to the beginning of the stream.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The stream </returns>
        public static Stream SeekToBegin(this Stream stream)
        {
            if (!stream.CanSeek) throw new InvalidOperationException("Stream does not support seeking.");
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        ///   Sets the stream cursor to the end of the stream.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The stream </returns>
        public static Stream SeekToEnd(this Stream stream)
        {
            if (!stream.CanSeek) throw new InvalidOperationException("Stream does not support seeking.");
            stream.Seek(0, SeekOrigin.End);
            return stream;
        }

        /// <summary>
        ///   Copies one stream into a another one.
        /// </summary>
        /// <param name="stream"> The source stream. </param>
        /// <param name="targetStream"> The target stream. </param>
        /// <param name="bufferSize"> The buffer size used to read / write. </param>
        /// <returns> The source stream. </returns>
        public static Stream CopyTo_(this Stream stream, Stream targetStream, int bufferSize)
        {
            if (!stream.CanRead) throw new InvalidOperationException("Source stream does not support reading.");
            if (!targetStream.CanWrite) throw new InvalidOperationException("Target stream does not support writing.");
            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0) targetStream.Write(buffer, 0, bytesRead);
            return stream;
        }

        /// <summary>
        ///   Copies one stream into a another one.
        /// </summary>
        /// <param name="stream"> The source stream. </param>
        /// <param name="targetStream"> The target stream. </param>
        /// <returns> The source stream. </returns>
        public static Stream CopyTo_(this Stream stream, Stream targetStream) { return stream.CopyTo_(targetStream, 4096); }

        /// <summary>
        ///   Copies any stream into a local MemoryStream
        /// </summary>
        /// <param name="stream"> The source stream. </param>
        /// <returns> The copied memory stream. </returns>
        public static MemoryStream CopyToMemory(this Stream stream)
        {
            var memoryStream = new MemoryStream((int) stream.Length);
            stream.CopyTo(memoryStream);
            return memoryStream;
        }

        /// <summary>
        ///   Reads the entire stream and returns a byte array.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <returns> The byte array </returns>
        /// <remarks>
        ///   Thanks to EsbenCarlsen  for providing an update to this method.
        /// </remarks>
        public static byte[] ReadAllBytes(this Stream stream) { using (var memoryStream = stream.CopyToMemory()) return memoryStream.ToArray(); }

        /// <summary>
        ///   Reads a fixed number of bytes.
        /// </summary>
        /// <param name="stream"> The stream to read from </param>
        /// <param name="sizeBuffer"> The number of bytes to read. </param>
        /// <returns> the read byte[] </returns>
        public static byte[] ReadFixedBuffersize(this Stream stream, int sizeBuffer)
        {
            var buffer = new byte[sizeBuffer];
            var offset = 0;
            do
            {
                var cnt = stream.Read(buffer, offset, sizeBuffer - offset);
                if (cnt == 0) return null;
                offset += cnt;
            } while (offset < sizeBuffer);
            return buffer;
        }

        /// <summary>
        ///   Writes all passed bytes to the specified stream.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="bytes"> The byte array / buffer. </param>
        public static void Write(this Stream stream, byte[] bytes) { stream.Write(bytes, 0, bytes.Length); }

        // ----------------------------------------
        /*
        public static IObservable<string> LineReader(this Stream stream, int bufferSize, char seperator,
                                                     bool includeSeperator)
        {
            if (null == stream) throw new ArgumentNullException("stream");
            if (1 > bufferSize) throw new ArgumentOutOfRangeException("bufferSize");
            var sub = new Subject<string>();
            // Wrap a MessageReader to read newline delimited chunks from stream.
            var delimiter = (byte) seperator;
            var obs = MessageReader(stream, delimiter, bufferSize, includeSeperator);
            obs.Subscribe(
                buf =>
                {
                    var s = Encoding.UTF8.GetString(buf);
                    sub.OnNext(s);
                },
                sub.OnError,
                sub.OnCompleted);
            return sub;
        }

        public static IObservable<byte[]> MessageReader(this Stream stream, byte delim, int bufferSize,
                                                        bool includeSeperator)
        {
            if (null == stream) throw new ArgumentNullException("stream");
            if (1 > bufferSize) throw new ArgumentOutOfRangeException("bufferSize");
            var sub = new Subject<byte[]>();
            var obs = ObservableStreamAsync(stream, bufferSize); // Read stream async in buf size chunks.
            byte[] buffer = null;
            obs.Subscribe(
                (buf) =>
                {
                    if (0 == buf.Length) return; // huh?
                    buf = buffer.ConcatBuf(buf);
                    buffer = null;
                    var lastIsDelim = (buf.Last() == delim);
                    var parts = buf.Split(delim, includeSeperator).ToArray();
                    for (var i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i];
                        // If on last element, send if it ended in the seperator. Otherwise buffer last part.
                        if (i == parts.Length - 1)
                            if (lastIsDelim) sub.OnNext(part);
                            else buffer = part;
                        else sub.OnNext(part);
                    }
                },
                (exp) => sub.OnError(exp),
                () =>
                {
                    // Check if we have stuff left in buffer and send.
                    if (null != buffer) sub.OnNext(buffer);
                    sub.OnCompleted();
                });
            return sub;
        }

        // Returns an observable stream. Publishes byte[] after each read completes.
        // byte[].Length <= bufferSize and >= 1
        // After reading completes, the Observable will signal OnComplete.
        public static IObservable<byte[]> ObservableStreamAsync(this Stream stream, int bufferSize)
        {
            if (null == stream) throw new ArgumentNullException("stream");
            var sub = new Subject<byte[]>();
            ReadHelper(stream, bufferSize, sub);
            Console.WriteLine("Kicked first read.");
            return sub;
        }

        static void ReadHelper(Stream stream, int bufferSize, IObserver<byte[]> sub)
        {
            var buf = new byte[bufferSize];
            try
            {
                stream.BeginRead(buf, 0, buf.Length,
                    (iAsyncResult) =>
                    {
                        try
                        {
                            var bytesRead = stream.EndRead(iAsyncResult);
                            if (bytesRead > 0)
                            {
                                // If we don't read bufferSize bytes, copy array to smaller size array.
                                // This way, the consumer of the service always sees only full arrays of data.
                                // This could have a resource tax if stream is constantantly returning < bufferSize bytes.
                                // This will normally only happen on the last read, but if happening more, you can tune the bufferSize.
                                // Alternatively, we could return a class/struct containing actual read Buffer and BytesRead
                                // properties and let the consumer inspect BytesRead properties.
                                if (bytesRead < bufferSize)
                                {
                                    var newBuf = new byte[bytesRead];
                                    Array.Copy(buf, newBuf, bytesRead);
                                    buf = newBuf;
                                }
                                sub.OnNext(buf);
                                // Post result before next read to prevent any overlap.
                                ReadHelper(stream, bufferSize, sub);
                            }
                            else sub.OnCompleted();
                            //Console.WriteLine("File closed.");
                        }
                        catch (Exception exp)
                        {
                            sub.OnError(exp);
                        }
                    }, null);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error.");
                sub.OnError(exp);
            }
        }

        // Test code

        public static void ObservableNetworkStream()
        {
            // Using Observable to read server stream.
            var server = new TcpListener(IPAddress.Any, 9000);
            var autoResetEnt = new AutoResetEvent(false);
            new Task(
                () =>
                {
                    server.Start();
                    autoResetEnt.Set();
                    var tcpClient = server.AcceptTcpClient();
                    var stream = tcpClient.GetStream();
                    var ob = stream.LineReader(100, '\n', true);
                    ob.Subscribe(
                        (str) =>
                        {
                            Console.WriteLine("Server: Msg from client:{0}", str);
                            // Echo it back to client.
                            var echo = Encoding.UTF8.GetBytes(str);
                            stream.Write(echo, 0, echo.Length);
                        },
                        (exp) =>
                        {
                            Console.WriteLine("Server: Error:{0}", exp.Message);
                            stream.Close();
                            tcpClient.Close();
                            server.Stop();
                        },
                        () =>
                        {
                            Console.WriteLine("Server: Client closed connection. Server stopped.");
                            stream.Close();
                            tcpClient.Close();
                            server.Stop();
                            Console.WriteLine("Server: Stopped.");
                        });
                }).Start();
            autoResetEnt.WaitOne();
            // Client
            var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 9000);
            var ns = client.GetStream();
            var obs = ns.LineReader(100, '\n', true);
            obs.Subscribe(
                (str) => Console.WriteLine("Client: Echo:{0}", str),
                (exp) => Console.WriteLine("Client: Error:{0}", exp.Message),
                () =>
                {
                    ns.Close();
                    client.Close();
                    Console.WriteLine("Client: TcpClient closed.");
                });
            var msg = Encoding.UTF8.GetBytes("Hello\nFrom\nObservable\n");
            ns.Write(msg, 0, msg.Length);
            client.Client.Shutdown(SocketShutdown.Send);
            Console.WriteLine("Client: Client wrote message.");
        }
        */
    }
}