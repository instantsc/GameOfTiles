using System;

namespace Game
{
    internal class Vision
    {
        private readonly World w;
        public Vision(World world)
        {
            w = world;
        }
        public bool Visible(Position dest, Position src)
        {
            double difY = dest.Y - src.Y;
            double difX = dest.X - src.X;
            if (Math.Abs(difY) < Math.Abs(difX))
            {
                var dir = Math.Sign(difX);
                var ydir = Math.Sign(difY);
                var inc = Math.Abs(difY/difX);
                var totalInc = 0.5;
                for (int i = src.X, j = src.Y; i != dest.X; i += dir)
                {
                    if (totalInc >= 1)
                    {
                        totalInc -= 1;
                        j += ydir;
                    }
                    if (!w.Field[i, j].Passable)
                    {
                        return false;
                    }
                    totalInc += inc;
                }
            }
            else
            {
                var dir = Math.Sign(difX);
                var ydir = Math.Sign(difY);
                var inc = Math.Abs(difX/difY);
                var totalInc = 0.5;
                for (int i = src.X, j = src.Y; j != dest.Y; j += ydir)
                {
                    if (totalInc >= 1)
                    {
                        totalInc -= 1;
                        i += dir;
                    }
                    if (!w.Field[i, j].Passable)
                    {
                        return false;
                    }
                    totalInc += inc;
                }
            }
            return (dest.X == src.X) || (dest.Y == src.Y);
        }
        public bool[,] VisionField(Unit u)
        {
            var result = new bool[w.Field.Width, w.Field.Height];
            var i0 = (int) Math.Max(0, u.Position.X - u.VisionDistance - 1);
            var i1 = (int) Math.Min(w.Field.Width, u.Position.X + u.VisionDistance + 2);
            var j0 = (int) Math.Max(0, u.Position.Y - u.VisionDistance - 1);
            var j1 = (int) Math.Min(w.Field.Height, u.Position.Y + u.VisionDistance + 2);

            for (var i = i0; i < i1; i++)
            {
                for (var j = j0; j < j1; j++)
                {
                    var pos = new Position(i, j);
                    var distance2 = Position.DistanceSqr(pos, u.Position);
                    result[i, j] = (distance2 <= u.VisionDistance*u.VisionDistance) && Visible(pos, u.Position);
                }
            }
            return result;
        }
    }
}