using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    [Serializable]
    public class Ship
    {
        public Orientation orientation;
        public Vector2i[] positions;
        public int size { get { return positions.Length; } }

        public Ship(int size)
        {
            positions = new Vector2i[size];
        }

        public bool IsOverlapping(Ship other)
        {
            foreach (Vector2i pos in positions)
            {
                foreach (Vector2i pos2 in other.positions)
                {
                    if (pos.Equals(pos2))
                        return true;
                }
            }
            return false;
        }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
