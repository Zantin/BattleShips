namespace BattleShipsLibrary
{
    public class AttackBoard
    {
        public int hits;
        public int misses;
        public int shots { get { return (hits + misses); } }

        private bool[,] attacked;
        private bool[,] hitOrMiss;

        public AttackBoard(int size)
        {
            attacked = new bool[size, size];
            hitOrMiss = new bool[size, size];
        }

        public void UpdateBoard(Vector2i pos, bool hit)
        {
            attacked[pos.x, pos.y] = true;
            hitOrMiss[pos.x, pos.y] = hit;

            if (hit)
                hits++;
            else
                misses++;
        }


    }
}
