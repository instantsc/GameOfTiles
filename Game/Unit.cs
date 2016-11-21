﻿namespace Game
{
    class Unit
    {
        public Unit(int x, int y, double visionDistance)
        {
            X = x;
            Y = y;
            VisionDistance = visionDistance;
        }
        public double Speed { get; }
        public double VisionDistance;
        public int X, Y;
        public Position Position => new Position(this);
    }
}
