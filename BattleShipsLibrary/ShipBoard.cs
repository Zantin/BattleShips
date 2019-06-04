using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    [Serializable]
    public class ShipBoard
    {
        public int size;
        public List<Ship> ships = new List<Ship>();
        public int shipsAlive { get { return ships.Count(x => x.isAlive); } }

        public ShipBoard(int size)
        {
            this.size = size;
        }

        public bool AddShip(Ship ship)
        {
            if (IsOverlapping(ship))
                return false;

            else
            {
                ships.Add(ship);
                return true;
            }
        }

        private bool IsOverlapping(Ship newShip)
        {
            foreach (Ship ship in ships)
            {
                if (newShip.IsOverlapping(ship))
                    return true;
            }
            return false;
        }

        public bool Attack(int x, int y)
        {
            return Attack(new Vector2i(x, y));
        }

        public bool Attack(Vector2i hitPos)
        {
            foreach (Ship ship in ships)
            {
                if (ship.Hit(hitPos))
                    return true;
            }
            return false;
        }
    }
}
