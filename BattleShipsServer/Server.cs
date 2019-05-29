using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace BattleShipsServer
{
    public class Server : BaseNetwork.BaseServer
    {
        public Server(int port) : base(port)
        {
        }

        public Server(IPAddress iPAddress, int port) : base(iPAddress, port)
        {
        }

        public Server(int port, Stream outputStream) : base(port, outputStream)
        {
        }

        public Server(IPAddress iPAddress, int port, Stream outputStream) : base(iPAddress, port, outputStream)
        {
        }

        public void Initialize()
        {
            
            base.Initialize(new ThreadStart(AwaitConnection));
        }

        private void AwaitConnection()
        {
            WriteLine("Hello From the server");
            //base.AwaitConnection();
        }
    }


}
