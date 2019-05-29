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
            Server server = new Server(2500, Console.OpenStandardOutput());
            Console.SetOut(server.writer);
            server.Initialize();
            server.Start();
            Console.ReadKey();
        }
    }

    
}
