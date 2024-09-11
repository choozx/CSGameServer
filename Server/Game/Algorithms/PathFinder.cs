using System;
using System.Numerics;
using Google.Protobuf.Protocol;
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
                GridPosition gFrom = ToGridPosition(from);
                GridPosition gTo = ToGridPosition(to);
                Path path = _pathFinder.FindPath(gFrom, gTo, mapInfo);
                if (path.Type == PathType.Complete && path.Edges.Count != 0)
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
        
        // public static IEdge FindPath2(int targetId, PositionInfo positionInfo, int mapId)
        // {
        //     Vector2 playerVector = PlayerManager.Instance.GetPlayerPosition(targetId);
        //     Vector2 monsterVector = new Vector2(positionInfo.PosX, positionInfo.PosY);
        //     
        //     SpecMap specMap = SpecManager.GetSpec<SpecMap>(typeof(SpecMap));
        //     Grid mapInfo = specMap.GetGrid(mapId);
        //     
        //     try
        //     {
        //         Path path = _pathFinder.FindPath(ToGridPosition(playerVector), ToGridPosition(monsterVector), mapInfo);
        //         if (path.Type == PathType.Complete && path.Edges.Count != 0)
        //         {
        //             IEdge des = path.Edges[0];
        //             return des;
        //         }
        //
        //         return null;
        //     }
        //     catch (Exception e)
        //     {
        //         // 맵 밖으로 나가면서 경로를 못찾는데... 원인이 유저의 좌표를 가져오는게 아닌 총알의 좌표를 가져오네? 왜?
        //         Console.WriteLine($"start:{playerVector}, end:{monsterVector}");
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }

        private static GridPosition ToGridPosition(Vector2 position)
        {
            return new GridPosition((int)position.X, (int)position.Y);
        }
    }
}