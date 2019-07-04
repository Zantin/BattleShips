using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using BattleShipsLibrary;
using BaseNetwork;

namespace BattleShips
{
    public class Connection
    {
        private Socket socket;
        private NetworkStream ns;
        private BinaryFormatter formatter = new BinaryFormatter();

        public bool isConnected { get { return socket.Connected; } }

        public Connection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(new IPAddress(new byte[] { 192, 168, 4, 250 }), 25000));

            ns = new NetworkStream(socket);
        }

        public bool Attack(Vector2i pos)
        {
            Network.SendData(ns, pos);
            return (ServerToClient)Network.GetEverything(ns)[0] == ServerToClient.Hit;
        }

        public void ThisIsMe(Player player)
        {
            Network.SendData(ns, player);
        }

        /*  Redo the network
         *  So that there is a thread listening to the network stream all the time
         *  That thread should alert the UI thread with updates.
         */
    }
}
