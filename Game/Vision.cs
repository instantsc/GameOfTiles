﻿using System;
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
            for (int i = 0; i < w.Field.Width; i++)
            {
                for (int j = 0; j < w.Field.Height; j++)
                {
                    var pos = new Position(i, j);
                    var distance = Position.Distance(pos, u.Position);
                    result[i, j] = distance <= u.VisionDistance && Visible(pos, u.Position);
                }
            }
            return result;
        }
    }
}
