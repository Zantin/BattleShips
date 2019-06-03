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
    }
}
