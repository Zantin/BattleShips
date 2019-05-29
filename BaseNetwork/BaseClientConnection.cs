using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseNetwork
{
    public abstract class BaseClientConnection
    {
        private Socket clientSocket;
        private Thread thread;
        private NetworkStream ns;
        private BaseServer server;

        private bool keepAlive = true;

        public BaseClientConnection(Socket clientSocket, BaseServer server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            this.ns = new NetworkStream(clientSocket);
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(HandleConnection));
            thread.Start();
        }

        public void Stop()
        {
            keepAlive = false;
        }

        public virtual BaseClientConnection GetThis()
        {
            return this;
        }

        protected virtual void HandleConnection()
        {
            throw new NotImplementedException("Don't use the default 'HandleConnection' function. Create your own");
        }

        protected virtual void Send(params object[] objects)
        {
            Network.SendData(ns, objects);
        }

        protected virtual List<object> ReadPacket()
        {
           return Network.GetEverything(ns);
        }

    }
}
