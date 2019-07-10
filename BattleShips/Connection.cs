using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using BattleShipsLibrary;
using BaseNetwork;
using System.Threading;
using System.IO;
using System.Windows;

namespace BattleShips
{
    public class Connection
    {
        private Socket socket;
        private NetworkStream ns;

        private Thread thread;

        public bool isConnected { get { return socket.Connected; } }

        private bool keepAlive = true;
        public bool isClosed = false;

        private List<ICallbackAble> subscribers = new List<ICallbackAble>();

        public Connection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (Options.serverIP == null || Options.serverPort == -1)
                Options.LoadOptions();

            socket.Connect(new IPEndPoint(new IPAddress(Options.serverIP), Options.serverPort));
            //socket.Connect(new IPEndPoint(new IPAddress(new byte[] { 192, 168, 0, 63 }), 25000));

            ns = new NetworkStream(socket);

            thread = new Thread(new ThreadStart(ThreadReader));
            thread.Start();
        }

        /*
        private void GetServerConnection()
        {
            if(File.Exists(Directory.GetCurrentDirectory() + "/options.txt"))
            {
                serverIP = new byte[4];
                var file = File.OpenText(Directory.GetCurrentDirectory() + "/options.txt");
                while(!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (line.Contains("IP"))
                    {
                        line = line.Split(':')[1];
                        var lines = line.Split('.');
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            if (i > 3)
                                break;
                            byte.TryParse(lines[i], out serverIP[i]);
                        }
                    }
                    else if(line.Contains("Port:"))
                    {
                        line = line.Split(':')[1];
                        int.TryParse(line.Trim(), out serverPort);
                    }
                }
            }
            else
            {
                CreateOptionsFile();
                MessageBox.Show("An options file has been generated.\nThe file is located together with the .exe to start the program.\nReplace the IP with the correct IP for the server you are trying to connect.");
            }
        }
        */

        

        public void Attack(Vector2i pos)
        {
            Network.SendData(ns, ClientToServer.Attack, pos);
        }

        public void ThisIsMe(Player player)
        {
            Network.SendData(ns, ClientToServer.ThisIsMe, player);
        }

        public void GiveUp()
        {
            Network.SendData(ns, ClientToServer.GiveUp);
        }

        private void ThreadReader()
        {
            while(keepAlive)
            {
                if(ns.DataAvailable)
                {
                    foreach(ICallbackAble callback in subscribers)
                    {
                        callback.Receive(Network.GetEverything(ns));
                    }
                }
                Thread.Sleep(100);
            }
            isClosed = true;
        }

        public void CloseConnection()
        {
            keepAlive = false;
        }

        public void Subscribe(ICallbackAble callback)
        {
            subscribers.Add(callback);
        }

        //            return (ServerToClient)Network.GetEverything(ns)[0] == ServerToClient.Hit;

        /*  Redo the network
         *  So that there is a thread listening to the network stream all the time
         *  That thread should alert the UI thread with updates.
         */
    }
}
