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
        protected Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Thread connectionThread;
        public bool isRunning { get; private set; }
        public List<BaseClientConnection> clients = new List<BaseClientConnection>();
        private Stream outputStream;
        private StreamWriter writer;
        private bool isInitialized = false;
        private bool isBound = false;
        private bool isListening = false;
        
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public BaseServer()
        { }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        private string ByteArrayToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString() + ".");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Bind the socket to a Network Interface
        /// </summary>
        /// <param name="ip">The ip to bind</param>
        /// <param name="port">The port to bind</param>
        public void Bind(IPAddress ip, int port)
        {
            Bind(new IPEndPoint(ip, port));
        }

        /// <summary>
        /// Bind the socket to a Network Interface
        /// </summary>
        /// <param name="endPoint">The ip and port to bind to</param>
        public void Bind(IPEndPoint endPoint)
        {
            serverSocket.Bind(endPoint);
            isBound = true;
        }

        /// <summary>
        /// Set the amount of connection that be in queue
        /// </summary>
        /// <param name="number">The amount</param>
        public void SetBacklog(int number)
        {
            serverSocket.Listen(number);
            isListening = true;
        }

        /// <summary>
        /// Returns the current outputStream
        /// Creates one if none existent
        /// </summary>
        public Stream GetStream()
        {
            if (outputStream == null)
                SetOutputStream(new MemoryStream(4096));
            return outputStream;
        }

        /// <summary>
        /// Changes the current outputStream to the one provided and create a new writer for that stream
        /// </summary>
        /// <param name="stream">The new stream</param>
        public void SetOutputStream(Stream stream)
        {
            outputStream = stream;
            writer = new StreamWriter(outputStream);
        }

        /// <summary>
        /// Returns the current writer
        /// Creates one if none existent
        /// </summary>
        public StreamWriter GetWriter()
        {
            if (writer == null)
                SetOutputStream(new MemoryStream(4096));
            return writer;
        }

        /// <summary>
        /// Initializes the server, allowing it to start
        /// </summary>
        /// <param name="awaitConnectionDelegate">The Method to run</param>
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
        /// Can only start if it's been bound to an IP and port, Is in listening state and Is Initialized
        /// </summary>
        public void Start()
        {
            bool error = false;

            if(!isListening)
            {
                error = true;
                WriteError("Server is not in Listening state");
            }
            if(!isBound)
            {
                error = true;
                WriteError("Server is not bound to an IP or Port");
            }
            if(!isInitialized)
            {
                error = true;
                WriteError("Server is not initialized!");
            }
            if(!error)
            {
                isRunning = true;
                connectionThread.Start();
            }
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
