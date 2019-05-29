using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BaseNetwork
{
    public abstract class BaseServer
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Thread connectionThread;
        public bool isRunning { get; }
        public List<BaseClientConnection> clients = new List<BaseClientConnection>();
        public Stream outputStream;
        public StreamWriter writer;
        protected bool isInitialized = false;
        
        /// <summary>
        /// Create a Server that listens on the ipAddress and port provided
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <param name="port"></param>
        public BaseServer(IPAddress iPAddress, int port, Stream outputStream)
        {
            serverSocket.Bind(new IPEndPoint(iPAddress, port));
            serverSocket.Listen(4);
            isRunning = true;
            if (outputStream != null)
                this.outputStream = outputStream;
            else
                this.outputStream = new BufferedStream(new MemoryStream(4096));
            writer = new StreamWriter(outputStream);
            writer.AutoFlush = true;
            WriteLine(String.Format("Listening on IP: {0} port: {1}", ByteArrayToString(iPAddress.GetAddressBytes()), port));
        }

        private string ByteArrayToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString() + ".");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Creates a Server that listens on the IP-Address and port provided
        /// </summary>
        /// <param name="iPAddress">The IP-Address of the Network Interface</param>
        /// <param name="port">The port to listen on</param>
        public BaseServer(IPAddress iPAddress, int port) : this(iPAddress, port, null)
        { }

        /// <summary>
        /// Creates a Server, With a redirected output, that listens on all available Network interfaces
        /// </summary>
        /// <param name="port">The port to listen on</param>
        public BaseServer(int port, Stream outputStream) : this(IPAddress.Any, port, outputStream)
        { }

        /// <summary>
        /// Creates a Server that listens on all available Network interfaces
        /// </summary>
        /// <param name="port">The port to listen on</param>
        public BaseServer(int port) : this(IPAddress.Any, port, null)
        { }

        public void Initialize(ThreadStart awaitConnectionDelegate)
        {
            if (awaitConnectionDelegate != null)
                connectionThread = new Thread(awaitConnectionDelegate);
            else
                connectionThread = new Thread(new ThreadStart(AwaitConnection));
           
            isInitialized = true;
        }

        /// <summary>
        /// Starts the server (Creates a new thread that waits for connections)
        /// </summary>
        public void Start()
        {
            if(isInitialized)
            {
                connectionThread.Start();
            }
            else
                WriteError("Server is not initialized!");
        }

        private void AwaitConnection()
        {
            WriteError("Don't use the base 'AwaitConnection'");
            WriteLine("You can fix this Error by sending your own ThreadStart with your await connection in the initialize");
        }

        /// <summary>
        /// Writes the provided object on to the outputStream
        /// </summary>
        /// <param name="obj"></param>
        public void Write(object obj)
        {
            writer.Write(obj.ToString() + " ");
        }

        /// <summary>
        /// Write the provided object on to the outputStream and ends the line
        /// </summary>
        /// <param name="obj"></param>
        public void WriteLine(object obj)
        {
            writer.WriteLine(obj.ToString());
        }

        public void WriteError(object obj)
        {
            WriteLine("ERROR: " + obj);
        }

        public void WriteInfo(object obj)
        {
            WriteLine("INFO: " + obj);
        }
    }
}
