using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseNetwork
{
    /// <summary>
    /// This is the Connection from the Server to the client
    /// </summary>
    public class BaseClientConnection
    {
        private Socket clientSocket;
        private Thread thread;
        private NetworkStream ns;

        private bool keepAlive = true;

        public BaseClientConnection(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
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
