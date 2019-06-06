using System.Threading;

namespace BattleShipsLibrary
{
    public class Game
    {
        public NetworkPlayer player1;
        public NetworkPlayer player2;

        private bool bothAlive = true;

        public Game(NetworkPlayer player1, NetworkPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        /// <summary>
        /// Starts the game.
        /// Run this method with a new thread since it is blocking until the game is over.
        /// </summary>
        public void Start()
        {
            Play();
        }

        private void Play()
        {

            while (bothAlive)
            {
                //This a full round
                Round(player1, player2);
                Round(player2, player1);
            }
        }

        private void NotifyPlayer(NetworkPlayer player)
        {
            player.clientConnection.Send(ServerToClient.YourTurn);
        }

        private void Round(NetworkPlayer currentPlayer, NetworkPlayer otherPlayer)
        {
            if (currentPlayer.player.shipBoard.shipsAlive > 0)
            {

                NotifyPlayer(currentPlayer);
                while (currentPlayer.player.nextAttack == null)
                {
                    Thread.Sleep(100);
                }
                if (Attack(currentPlayer.player.nextAttack, otherPlayer))
                    currentPlayer.clientConnection.Send(ServerToClient.Hit);
                else
                    currentPlayer.clientConnection.Send(ServerToClient.Miss);

                if (otherPlayer.player.shipBoard.shipsAlive <= 0)
                {
                    currentPlayer.clientConnection.Send(ServerToClient.Win);
                    otherPlayer.clientConnection.Send(ServerToClient.Loss);
                    bothAlive = false;
                }
                currentPlayer.player.nextAttack = null;
            }
        }

        private bool Attack(Vector2i attackPos, NetworkPlayer otherPlayer)
        {
            return otherPlayer.player.shipBoard.Attack(attackPos);

            //otherPlayer.clientConnection.Send(ServerToClient.Attack, attackPos);
        }

        private NetworkPlayer GetOtherPlayer(NetworkPlayer currentPlayer)
        {
            return currentPlayer == player1 ? player2 : player1;
        }


    }


}
