using System;

namespace BattleShipsLibrary
{
    [Serializable]
    public class Player
    {
        public ShipBoard shipBoard;

        public Vector2i nextAttack;

        public int wins { get; private set; }
        public int loss { get; private set; }
        public int games { get { return wins + loss; } }

        public int hits { get; private set; }
        public int miss { get; private set; }
        public int shots { get { return hits + miss; } }

        public bool isGivingUp = false;

        public string username { get; set; }

        public bool isAlive { get { return shipBoard.shipsAlive > 0; } }

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
