using System;
using System.Numerics;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using Server.Game.Spec;

namespace Server.Game.Algorithms
{
    public class Navigation
    {
        private static PathFinder _pathFinder = new PathFinder();

        public static IEdge FindPath(Vector2 from, Vector2 to, int mapId)
        {
            SpecMap specMap = SpecManager.GetSpec<SpecMap>(typeof(SpecMap));
            Grid mapInfo = specMap.GetGrid(mapId);
            
            try
            {
                Path path = _pathFinder.FindPath(ToGridPosition(from), ToGridPosition(to), mapInfo);
                if (path.Type == PathType.Complete)
                {
                    IEdge des = path.Edges[0];
                    return des;   
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"start:{from.X},{from.Y}, end:{to.X},{to.Y}");
                Console.WriteLine(e);
                throw;
            }
        }

        private static GridPosition ToGridPosition(Vector2 position)
        {
            return new GridPosition((int)position.X, (int)position.Y);
        }
    }
}