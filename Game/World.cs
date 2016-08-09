using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X, Y;

        public Position MoveXUp => new Position(X + 1, Y);
        public Position MoveXDown => new Position(X - 1, Y);
        public Position MoveYUp => new Position(X, Y + 1);
        public Position MoveYDown => new Position(X, Y - 1);

        public static double Distance(Position pos1, Position pos2) => Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
    class Tile : PriorityQueue.FastPriorityQueueNode, IComparable<Tile>
    {
        public Tile(Position pos, bool passable = true)
        {
            Position = pos;
            Passable = passable;
        }
        public Position Position { get; set; }
        public bool Passable { get; set; }
        public int CompareTo(Tile other)
        {
            if (other.Position.X == Position.X)
            {
                return other.Position.Y - Position.Y;
            }
            return other.Position.X - Position.X;
        }
    }

    class Field
    {
        public Field(Tile[,] tiles)
        {
            Height = tiles.GetLength(1);
            Width = tiles.GetLength(0);
            Tiles = tiles;
        }
        public Tile this[Position pos]
        {
            get { return Tiles[pos.X, pos.Y]; }
        }
        public Tile[,] Tiles { get; }
        public int Height { get; } //Y
        public int Width { get; } //X
        public List<Tile> Neighbours(Tile tile)
        {
            return Neighbours(tile.Position);
        }
        public List<Tile> Neighbours(Position pos)
        {
            List<Tile> result = new List<Tile>();
            if (pos.X > 0)
                result.Add(this[pos.MoveXDown]);
            if (pos.X < Width - 1)
                result.Add(this[pos.MoveXUp]);
            if (pos.Y > 0)
                result.Add(this[pos.MoveYDown]);
            if (pos.Y < Height - 1)
                result.Add(this[pos.MoveYUp]);
            return result;
        }

    }
    partial class World
    {
        public HashSet<Unit> Units;
        public World(Field field)
        {
            Field = field;
        }
        public Field Field { get; }

    }
}
