using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    [Serializable]
    public class AttackBoard
    {
        public int size;
        public int hits;
        public int misses;
        public int shots { get { return (hits + misses); } }
    }
}
