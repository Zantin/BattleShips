using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaseNetwork;
using BattleShipsLibrary;

namespace BattleShipsServer
{
    public class MatchmakingServer : BaseServer
    {
        private Random rand = new Random();
        private List<Game> aktiveGames = new List<Game>();
        private List<ClientConnection> clients = new List<ClientConnection>();
        public MatchmakingServer()
        { }

        public void Initialize()
        {
            base.Initialize(new ThreadStart(AwaitConnection));
        }

        private void AwaitConnection()
        {
            WriteLine("Hello From the matchmaking server");
            while(isRunning)
            {
                clients.Add(new ClientConnection(serverSocket.Accept()));
                clients.Last().Start();
                WriteLine("New connection from: " + clients.Last().ip);
                while(clients.Count >= 2)
                {
                    WriteLine("New Game Started");
                    ClientConnection player1 = GetRandomPlayer();
                    ClientConnection player2 = GetRandomPlayer();

                    Game game = new Game(new NetworkPlayer(player1), new NetworkPlayer(player2), this);
                    game.Start();
                    aktiveGames.Add(game);
                }
            }
        }

        private ClientConnection GetRandomPlayer()
        {
            ClientConnection player = clients.ElementAt(rand.Next(clients.Count -1));
            clients.Remove(player);
            return player;
        }

        public void EndGame(Game game)
        {
            aktiveGames.Remove(game);
        }
    }
}
