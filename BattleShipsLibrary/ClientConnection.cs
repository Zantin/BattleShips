using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BattleShipsLibrary
{
    public class ClientConnection : BaseNetwork.BaseClientConnection
    {
        private List<object> packet;
        private Player player;
        private Game game;

        public ClientConnection(Socket clientSocket) : base(clientSocket)
        {
            Initialize(new ThreadStart(HandleConnection));
        }

        protected override void HandleConnection()
        { 
            while (keepAlive)
            {
                if(ns.DataAvailable)
                {
                    packet = ReadPacket();
                    HandleCommand();
                }
                Thread.Sleep(100);
            }
        }

        private void HandleCommand()
        {
            switch ((Commands)packet[1])
            {
                case Commands.Attack:
                    player.nextAttack = (Vector2i)packet[2];
                    break;
                default:
                    break;
            }
        }

        public void SendEnemyUsername(Player enemyPlayer)
        {
            SendEnemyUsername(enemyPlayer.username);
        }

        public void SendEnemyUsername(string username)
        {
            Send(Commands.EnemyUsername, username);
        }

        public void SetGame(Game game)
        {
            this.game = game;
        }


    }
}