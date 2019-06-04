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
        public bool isAlive { get { return life <= 0; } }
        public int life;

        public Ship(int size)
        {
            positions = new Vector2i[size];
            life = size;
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

        /// <summary>
        /// Returns whether or not the attack hit the ship, If it hits a life is deducted
        /// </summary>
        /// <param name="hitPos">The 'x','y' cordinate of the attack</param>
        public bool IsHit(Vector2i hitPos)
        {
            foreach(Vector2i pos in positions)
            {
                if(hitPos.Equals(pos))
                {
                    life--;
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
