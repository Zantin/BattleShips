using System;
using System.Linq;

namespace BattleShipsLibrary
{
    [Serializable]
    public class Ship
    {
        public Orientation orientation;
        public ShipPart[] shipParts;
        public int size { get { return shipParts.Length; } }
        public bool isAlive { get { return shipParts.Count(x => x.isHit == false) > 0; } }

        public Ship(int size)
        {
            shipParts = new ShipPart[size];
        }

        public bool IsOverlapping(Ship other)
        {
            foreach (ShipPart shipPart in shipParts)
            {
                foreach (ShipPart otherShipPart in other.shipParts)
                {
                    if (shipPart.position.Equals(otherShipPart.position))
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
            foreach(ShipPart shipPart in shipParts)
            {
                if(hitPos.Equals(shipPart.position))
                {
                    shipPart.Hit();
                    return true;
                }
            }
            return false;
        }

    }

    [Serializable]
    public class ShipPart
    {
        public Vector2i position { get; private set; }
        public bool isHit { get; private set; }

        public ShipPart(Vector2i position)
        {
            this.position = position;
            isHit = false;
        }

        public void Hit()
        {
            isHit = true;
        }

        public void Reset()
        {
            isHit = false;
        }

        public void NewPosition(Vector2i position)
        {
            this.position = position;
        }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
