using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsLibrary
{
    [Serializable]
    public class Vector2i
    {
        public int x;
        public int y;

        public Vector2i()
        { }

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Vector2i other = (Vector2i)obj;
                return (x == other.x && y == other.y);
            }
        }
    }
}
