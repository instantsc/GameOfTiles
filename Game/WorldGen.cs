using System;
using System.Collections.Generic;
using System.Linq;

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
                    tiles[i, j] = new Tile(new Position(i, j), false);
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
                Tile t = s.ElementAt(r.Next(Math.Min(s.Count, 3)));
                s.Remove(t);
                var nntiles = field.Neighbours(t);
                var count = nntiles.Count(x => x.Passable);
                if (count <= 1 || (count == 2 && (nntiles.Count < 4 || r.Next() < 0.1)))
                {
                    t.Passable = true;
                    foreach (var tile in nntiles.Where(x => !x.Passable))
                    {
                        s.Add(tile);
                    }
                }
            }
            for (int i = width / 2 - width / 10; i < width / 2 + width / 10; i++)
            {
                for (int j = height / 2 - height / 10; j < height / 2 + height / 10; j++)
                {
                    tiles[i, j].Passable = true;
                }
            }
            List<Unit> units = new List<Unit>();
            Unit p1 = new Unit(3, 3, 10);
            Unit p2 = new Unit(3, 4, 10);
            units.Add(p1);
            units.Add(p2);
            return new World(field, units);
        }
    }
}
