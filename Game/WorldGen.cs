using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    partial class World
    {
        public static World Generate(int width, int height, int seed = 0)
        {
            Tile[,] tiles = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i,j]=new Tile(new Position(i, j),false);
                }
            }
            Field field = new Field(tiles);
            Random r = seed == 0 ? new Random() : new Random(seed);
            Position start = new Position(r.Next(width), r.Next(height));
            field[start].Passable = true;
            SortedSet<Tile> s = new SortedSet<Tile>();
            var ntiles = field.Neighbours(start);
            foreach (var tile in ntiles)
            {
                s.Add(tile);
            }
            while (s.Count != 0)
            {
                Tile t = s.ElementAt(r.Next(s.Count));
                s.Remove(t);
                var nntiles = field.Neighbours(t);
                var count = nntiles.Count(x => x.Passable);
                if (count<= 1||(count <= 2 && r.NextDouble() < 0.5))
                {
                    t.Passable = true;
                    foreach (var tile in nntiles.Where(x => !x.Passable))
                    {
                        s.Add(tile);
                    }
                }
            }
            return new World(field);
        }
    }
}
