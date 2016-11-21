using System.Collections.Generic;
using System.Linq;
using Game.PriorityQueue;

namespace Game
{
    class Path
    {
        
    }

    interface IPather
    {
       LinkedList<Tile> FindPath(World w, Tile start, Tile end);
    }
    class AStar:IPather
    {
        public static readonly AStar Pather=new AStar();
        private AStar()
        {
            
        }
        public LinkedList<Tile> FindPath(World w,Tile start, Tile end)
        {
            LinkedList<Tile> result=new LinkedList<Tile>();
            double priority = Position.DistanceSqr(start.Position, end.Position);
            Dictionary<Tile,double> travelCost=new Dictionary<Tile, double>();
            Dictionary<Tile,Tile> previousTile=new Dictionary<Tile, Tile>();
            travelCost[start] = 0;
            FastPriorityQueue<Tile> predictedCost=new FastPriorityQueue<Tile>(w.Field.Height*w.Field.Width);
            HashSet<Tile> closed=new HashSet<Tile>();
            predictedCost.Enqueue(start,priority);
            while (predictedCost.Count != 0)
            {
                Tile current = predictedCost.Dequeue();
                closed.Add(current);
                if (current == end)
                {
                    while (current != start)
                    {
                        result.AddFirst(current);
                        current = previousTile[current];
                    }
                    return result;
                }
                foreach (var neighbour in w.Field.Neighbours(current).Where(x=>x.Passable))
                {
                    if (!closed.Contains(neighbour))
                    {
                        double newCost = travelCost[current] + Position.DistanceSqr(current.Position, neighbour.Position);
                        if (predictedCost.Contains(neighbour))
                        {
                            if(newCost>=travelCost[neighbour])
                                continue;
                            predictedCost.UpdatePriority(neighbour,newCost+Position.DistanceSqr(neighbour.Position,end.Position));
                        }
                        else
                        {
                            predictedCost.Enqueue(neighbour,newCost+Position.DistanceSqr(neighbour.Position,end.Position));
                        }
                        travelCost[neighbour] = newCost;
                        previousTile[neighbour] = current;
                    }
                }
            }
            return null;
        }
    }
}
