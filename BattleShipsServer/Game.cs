using BattleShipsLibrary;
using System.Threading;

namespace BattleShipsServer
{
    public class Game
    {
        public NetworkPlayer player1;
        public NetworkPlayer player2;

        private MatchmakingServer server;

        public Thread gameThread;

        private bool bothAlive
        {
            get
            {
                if (player1.player != null && player2.player != null)
                    return (player1.player.isAlive && player2.player.isAlive);
                else
                    return true;
            }
        }

        public Game(NetworkPlayer player1, NetworkPlayer player2, MatchmakingServer server)
        {
            this.server = server;
            gameThread = new Thread(new ThreadStart(Play));

            this.player1 = player1;
            this.player2 = player2;

            
            
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            player1.Send(ServerToClient.BattleReady);
            player2.Send(ServerToClient.BattleReady);

            gameThread.Start();
        }

        private void Play()
        {

            while (bothAlive)
            {
                //This a full round
                if(player1.player.isAlive)
                    Round(player1, player2);
                if(player2.player.isAlive)
                    Round(player2, player1);
            }
            if(player1.player.isAlive && !player2.player.isAlive)
            {
                player1.Send(ServerToClient.Win);
                player2.Send(ServerToClient.Loss);
            }
            else if (!player1.player.isAlive && player2.player.isAlive)
            {
                player2.Send(ServerToClient.Win);
                player1.Send(ServerToClient.Loss);
            }
            else
            {
                player1.Send(ServerToClient.Loss);
                player2.Send(ServerToClient.Loss);
            }
            player1.Send(ServerToClient.EnemyShips, player2.player.shipBoard);
            player2.Send(ServerToClient.EnemyShips, player1.player.shipBoard);
            server.EndGame(this);
        }

        private void NotifyPlayer(NetworkPlayer player)
        {
            player.clientConnection.Send(ServerToClient.YourTurn);
        }

        private void Round(NetworkPlayer currentPlayer, NetworkPlayer otherPlayer)
        {
            if(currentPlayer.player.isGivingUp)
            {
                currentPlayer.player.isGivingUp = false;
                currentPlayer.Send(ServerToClient.Loss);
                otherPlayer.Send(ServerToClient.Win);
            }
            else if (currentPlayer.player.shipBoard.shipsAlive > 0)
            {

                NotifyPlayer(currentPlayer);

                while (currentPlayer.player.nextAttack == null)
                {
                    Thread.Sleep(100);
                }

                if (Attack(currentPlayer.player.nextAttack, otherPlayer))
                {
                    currentPlayer.clientConnection.Send(ServerToClient.Hit, currentPlayer.player.nextAttack);
                    otherPlayer.Send(ServerToClient.EnemyAttack, currentPlayer.player.nextAttack, true);
                }
                else
                {
                    currentPlayer.clientConnection.Send(ServerToClient.Miss, currentPlayer.player.nextAttack);
                    otherPlayer.Send(ServerToClient.EnemyAttack, currentPlayer.player.nextAttack, false);
                }


                /*
                if (otherPlayer.player.shipBoard.shipsAlive <= 0)
                {
                    currentPlayer.clientConnection.Send(ServerToClient.Win);
                    otherPlayer.clientConnection.Send(ServerToClient.Loss);
                    bothAlive = false;
                }
                */
                currentPlayer.player.nextAttack = null;
            }
        }

        private bool Attack(Vector2i attackPos, NetworkPlayer otherPlayer)
        {
            return otherPlayer.player.shipBoard.Attack(attackPos);

            //otherPlayer.clientConnection.Send(ServerToClient.Attack, attackPos);
        }

        /*
        private NetworkPlayer GetOtherPlayer(NetworkPlayer currentPlayer)
        {
            return currentPlayer == player1 ? player2 : player1;
        }
        */


    }


}
