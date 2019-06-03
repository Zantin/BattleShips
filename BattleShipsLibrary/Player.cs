using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    [Serializable]
    public class Player
    {
        public AttackBoard attackBoard;
        public ShipBoard shipBoard;

        public int wins { get; private set; }
        public int loss { get; private set; }
        public int games { get { return wins + loss; } }

        public int hits { get; private set; }
        public int miss { get; private set; }
        public int shots { get { return hits + miss; } }

        public string username { get; set; }

        public Player()
        {
            wins = 0;
            loss = 0;
        }

        public void AddWin()
        {
            wins++;
        }

        public void AddLoss()
        {
            loss++;
        }

        public void AddHits(int amount)
        {
            hits += amount;
        }

        public void AddMiss(int amount)
        {
            miss += amount;
        }

    }
}
