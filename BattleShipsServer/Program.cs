using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace BattleShipsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MatchmakingServer matchmakingServer = new MatchmakingServer();
            matchmakingServer.SetOutputStream(Console.OpenStandardOutput());
            Console.SetOut(matchmakingServer.GetWriter());
            matchmakingServer.Bind(IPAddress.Any, 25000);
            matchmakingServer.SetBacklog(4);
            matchmakingServer.Initialize();
            matchmakingServer.Start();
            while (Console.ReadKey(true).KeyChar != 'q')
            { }
            Console.WriteLine("Stoping Server");
        }
    }

    
}
