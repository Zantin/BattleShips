using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace BattleShipsLibrary
{
    public class ClientConnection : BaseNetwork.BaseClientConnection
    {
        private List<object> packet;
        private NetworkPlayer networkPlayer;

        public ClientConnection(Socket clientSocket) : base(clientSocket)
        {
            
            Initialize(new ThreadStart(HandleConnection));
        }

        public void BindNetworkPlayer(NetworkPlayer networkPlayer)
        {
            this.networkPlayer = networkPlayer;
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
            switch ((ClientToServer)packet[1])
            {
                case ClientToServer.Attack:
                    networkPlayer.player.nextAttack = (Vector2i)packet[2];
                    break;
                case ClientToServer.GiveUp:
                    break;
                case ClientToServer.ThisIsMe:
                    networkPlayer.SetPlayer((Player)packet[2]);
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
            Send(ServerToClient.EnemyUsername, username);
        }

    }
}