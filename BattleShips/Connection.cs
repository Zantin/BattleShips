﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using BattleShipsLibrary;
using BaseNetwork;
using System.Threading;

namespace BattleShips
{
    public class Connection
    {
        private Socket socket;
        private NetworkStream ns;

        private Thread thread;

        public bool isConnected { get { return socket.Connected; } }

        private bool keepAlive = true;

        private List<ICallbackAble> subscribers = new List<ICallbackAble>();

        public Connection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(new IPAddress(new byte[] { 192, 168, 4, 250 }), 25000));

            ns = new NetworkStream(socket);

            thread = new Thread(new ThreadStart(ThreadReader));
            thread.Start();
        }

        public void Attack(Vector2i pos)
        {
            Network.SendData(ns, pos);
        }

        public void ThisIsMe(Player player)
        {
            Network.SendData(ns, player);
        }

        private void ThreadReader()
        {
            while(keepAlive)
            {
                if(ns.DataAvailable)
                {
                    foreach(ICallbackAble callback in subscribers)
                    {
                        callback.Receive()
                    }
                }
                Thread.Sleep(100);
            }
        }

        public void Subscribe(ICallbackAble callback)
        {

        }

        //            return (ServerToClient)Network.GetEverything(ns)[0] == ServerToClient.Hit;

        /*  Redo the network
         *  So that there is a thread listening to the network stream all the time
         *  That thread should alert the UI thread with updates.
         */
    }
}
