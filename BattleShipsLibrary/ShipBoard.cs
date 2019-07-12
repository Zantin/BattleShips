using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipsLibrary
{
    [Serializable]
    public class ShipBoard
    {
        public int boardSize;
        public List<Ship> ships = new List<Ship>();
        public int shipsAlive { get { return ships.Count(x => x.isAlive); } }

        /// <summary>
        /// X component is the ship Size
        /// Y component is the amount of ships allowed
        /// </summary>
        public Vector2i[] allowedShips;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardSize">The Size of the board</param>
        /// <param name="allowedShips">An object containing the ship size and the amount of that ship that is allowed</param>
        public ShipBoard(int boardSize, params Vector2i[] allowedShips)
        {
            this.boardSize = boardSize;
            this.allowedShips = allowedShips;
        }

        public bool AddShip(Ship ship)
        {
            if (ship.orientation == Orientation.Horizontal)
                if (ship.shipParts[0].position.x + ship.size > boardSize)
                    return false;
            if (ship.orientation == Orientation.Horizontal)
                if (ship.shipParts[0].position.x < 0)
                    return false;
            if (ship.orientation == Orientation.Vertical)
                if (ship.shipParts[0].position.y + ship.size > boardSize)
                    return false;
            if (ship.orientation == Orientation.Vertical)
                if (ship.shipParts[0].position.y < 0)
                    return false;
            if (IsOverlapping(ship))
                return false;


            if(IsSizeAllowed(ship))
            {
                ships.Add(ship);
                return true;
            }
            return false;
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

        private bool IsSizeAllowed(Ship newShip)
        {
            if(allowedShips.Count() > 0)
            {
                var temp = allowedShips.FirstOrDefault(x => x.x == newShip.size);
                if(temp != null)
                {
                    if (allowedShips.FirstOrDefault(x => x.x == newShip.size).y > GetAmountOfShipOfSize(newShip.size))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Returns whether a ship is hit or not at the provided position
        /// </summary>
        /// <param name="x">The 'x' cordinate</param>
        /// <param name="y">The 'y' cordinate</param>
        public bool Attack(int x, int y)
        {
            return Attack(new Vector2i(x, y));
        }

        /// <summary>
        /// Returns whether a ship is hit or not at the provided position
        /// </summary>
        /// <param name="hitPos">The position to test for</param>
        public bool Attack(Vector2i hitPos)
        {
            foreach (Ship ship in ships)
            {
                if(ship.isAlive)
                    if (ship.IsHit(hitPos))
                        return true;
            }
            return false;
        }

        public int GetAmountOfShipOfSize(int size)
        {
            return ships.Count(x => x.size == size);
        }

        public int GetAmountOfShipAllowedOfSize(int size)
        {
            var t = allowedShips.FirstOrDefault(x => x.x == size);
            if (t != null)
                return t.y;
            else
                return 0;

        }

        public Ship ContainsShip(int x, int y)
        {
            return ContainsShip(new Vector2i(x, y));
        }

        public Ship ContainsShip(Vector2i pos)
        {
            foreach (Ship ship in ships)
            {
                foreach (ShipPart part in ship.shipParts)
                {
                    if (part.position.Equals(pos))
                        return ship;
                }
            }
            return null;
        }

        public void RemoveShip(Ship ship)
        {
            ships.Remove(ship);
        }

        public bool IsAllShipsPlaced()
        {
            foreach (Vector2i allowedShip in allowedShips)
            {
                if (allowedShip.y != ships.Count(x => x.size == allowedShip.x))
                    return false;
            }
            return true;
        }
    }
}
