using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    public enum ClientToServer
    {
        Attack,
        GiveUp,
    }

    public enum ServerToClient
    {
        Win,
        Loss,
        EnemyUsername,
        Hit,
        Miss,
        YourTurn,
    }
}
