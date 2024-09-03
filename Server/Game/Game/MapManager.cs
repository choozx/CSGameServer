using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    
    public class MapManager
    {
        public static MapManager Instance { get; } = new MapManager();
        private Dictionary<int, Map> _maps = new Dictionary<int, Map>();
        
        public MapManager()
        {
            // TODO _map 셋팅
            Map map = new Map();
            _maps.Add(1, map);
        }

        public bool CanGo(PositionInfo positionInfo, int mapId)
        {
            Map map = _maps[mapId];
            return map.CanGo(positionInfo);
        }

        public Vector2 DecideDirection(PositionInfo pos1, PositionInfo pos2)
        {
            /*
             * A* 알고르즘 사용 해야됨. 다만 드는 생각이 경로를 끝까지 탐색해야하나 싶음
             * 어차피 방향백터만 알면 되기때문에 첫 방향만 리턴하면 될듯
             */
            
            return new Vector2();
        }
    }
}