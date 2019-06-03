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
    public class MatchmakingServer : BaseNetwork.BaseServer
    {
        private Random rand = new Random();
        private List<Socket> clients = new List<Socket>();
        private List<Game> aktiveGames = new List<Game>();
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
                clients.Add(serverSocket.Accept());
                while(clients.Count >= 2)
                {
                    Socket player1 = GetRandomPlayer();
                    
                    Socket player2 = GetRandomPlayer();
                    aktiveGames.Add(new Game(new NetworkPlayer(new BaseClientConnection(player1)), new NetworkPlayer(new BaseClientConnection(player2))));

                }
            }
        }

        private Socket GetRandomPlayer()
        {
            Socket player = clients.ElementAt(rand.Next(clients.Count -1));
            clients.Remove(player);
            return player;
        }
    }
}
