using BaseNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    public class NetworkPlayer
    {
        public Player player;
        public ClientConnection clientConnection;

        //Player connects to the server, and sends their username

        public NetworkPlayer(ClientConnection clientConnection)
        {
            this.clientConnection = clientConnection;
            player = new Player();
        }


    }
}
