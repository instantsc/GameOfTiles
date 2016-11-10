using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Vision
    {
        private World w;
        public Vision(World world)
        {
            this.w = world;
        }
        public bool Visible(Position dest, Position src)
        {
            double difY = dest.Y - src.Y;
            double difX = dest.X - src.X;
            if (Math.Abs(difY) < Math.Abs(difX))
            {
                int dir = Math.Sign(difX);
                int ydir = Math.Sign(difY);
                double inc = Math.Abs(difY / difX);
                double totalInc = 0.5;
                for (int i = src.X, j = src.Y; i != dest.X; i += dir)
                {
                    if (totalInc >= 1)
                    {
                        totalInc -= 1;
                        j += ydir;
                    }
                    if (!w.Field[i, j].Passable)
                        return false;
                    totalInc += inc;
                }
            }
            else
            {
                int dir = Math.Sign(difX);
                int ydir = Math.Sign(difY);
                double inc = Math.Abs(difX / difY);
                double totalInc = 0.5;
                for (int i = src.X, j = src.Y; j != dest.Y; j += ydir)
                {
                    if (totalInc >= 1)
                    {
                        totalInc -= 1;
                        i += dir;
                    }
                    if (!w.Field[i, j].Passable)
                        return false;
                    totalInc += inc;
                }
            }
            return true;
        }
        public bool[,] VisionField(Unit u)
        {
            bool[,] result = new bool[w.Field.Width, w.Field.Height];
            int i0 = (int)Math.Max(0, u.Position.X - u.VisionDistance - 1);
            int i1 = (int)Math.Min(w.Field.Width, u.Position.X + u.VisionDistance + 2);
            int j0 = (int)Math.Max(0, u.Position.Y - u.VisionDistance - 1);
            int j1 = (int)Math.Min(w.Field.Height, u.Position.Y + u.VisionDistance + 2);

            for (int i = i0; i < i1; i++)
            {
                for (int j = j0; j < j1; j++)
                {
                    var pos = new Position(i, j);
                    var distance2 = Position.DistanceSqr(pos, u.Position);
                    result[i, j] = distance2 <= u.VisionDistance*u.VisionDistance && Visible(pos, u.Position);
                }
            }
            return result;
        }
    }
}
