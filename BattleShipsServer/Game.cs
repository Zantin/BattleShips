using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using BattleShipsLibrary;

namespace BattleShipsServer
{
    public class Game
    {
        public NetworkPlayer player1;
        public NetworkPlayer player2;

        public Game(NetworkPlayer player1, NetworkPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }
        
    }


}
