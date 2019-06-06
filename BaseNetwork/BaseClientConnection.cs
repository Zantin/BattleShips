using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace BaseNetwork
{
    /* Rewrite this class so it only handles the network part.
     * There by only sending packets and returning read packets
     */


    /// <summary>
    /// This is the Connection from the Server to the client
    /// </summary>
    public abstract class BaseClientConnection
    {
        protected Socket clientSocket;
        private Thread thread;
        protected NetworkStream ns;
        protected BaseServer server;

        protected bool keepAlive { get; private set; } = true;
        protected bool isInitialized = false;

        public BaseClientConnection(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            this.ns = new NetworkStream(clientSocket);
        }

        public void Initialize(ThreadStart handleConnectionDelegate)
        {
            if (handleConnectionDelegate != null)
                thread = new Thread(new ThreadStart(handleConnectionDelegate));
            else
                thread = new Thread(new ThreadStart(HandleConnection));

            isInitialized = true;
        }

        public void SetBaseServer(BaseServer server)
        {
            this.server = server;
        }

        public void Start(ThreadStart threadStart)
        {
            if (isInitialized)
                thread.Start();
            else
                throw new Exception("Connection not Initialized");
        }

        public void Stop()
        {
            keepAlive = false;
        }

        protected virtual void HandleConnection()
        {
            throw new NotImplementedException("Don't use the default 'HandleConnection' function. Create your own");
        }

        public virtual void Send(params object[] objects)
        {
            Network.SendData(ns, objects);
        }

        protected List<object> ReadPacket()
        {
           return Network.GetEverything(ns);
        }
        
    }
}
