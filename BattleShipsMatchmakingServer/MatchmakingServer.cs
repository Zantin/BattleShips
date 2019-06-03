using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsServer
{
    public class MatchmakingServer : BaseNetwork.BaseServer
    {
        public MatchmakingServer(int port) : base(port)
        {
        }

        public MatchmakingServer(IPAddress iPAddress, int port) : base(iPAddress, port)
        {
        }

        public MatchmakingServer(int port, Stream outputStream) : base(port, outputStream)
        {
        }

        public MatchmakingServer(IPAddress iPAddress, int port, Stream outputStream) : base(iPAddress, port, outputStream)
        {
        }
    }
}
