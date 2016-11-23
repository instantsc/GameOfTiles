namespace Game
{
    internal class Unit
    {
        public readonly double VisionDistance;
        public int X, Y;
        public Unit(int x, int y, double visionDistance)
        {
            X = x;
            Y = y;
            VisionDistance = visionDistance;
        }
        public Position Position => new Position(this);
    }
}