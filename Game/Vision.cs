using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Vision
    {
        public bool Visible(Position dest, Position src, World w)
        {
            double difY = dest.Y - src.Y;
            double difX = dest.X - src.X;
            if (Math.Abs(difY) < Math.Abs(difX))
            {
                int dir = Math.Sign(difX);
                int ydir = Math.Sign(difY);
                double inc = Math.Abs(difY/difX);
                double totalInc = 0.5;
                for (int i = src.X,j=src.Y; i != dest.X; i += dir)
                {
                    if (totalInc >= 1)
                    {
                        totalInc -= 1;
                        j+=ydir;
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
    }
}
