using System.Collections.Generic;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public struct Tile
    {
        public short _x;
        public short _y;

        public Tile(float positionInfoPosX, float positionInfoPosY)
        {
            _x = (short)positionInfoPosX;
            _y = (short)positionInfoPosY;
        }
    }
    
    public class Map
    {
        public int _mapId;

        private Dictionary<Tile, bool> _tiles = new Dictionary<Tile, bool>();

        public Map()
        {
            _mapId = 1;
            
            // 임시 울타리 생성
            for (int i = -299; i < 299; i++)
            {
                for (int j = -299; j < 299; j++)
                {
                    Tile tile = new Tile(i, j);
                    _tiles.Add(tile, true);   
                }
            }
        }
        
        public bool CanGo(PositionInfo positionInfo)
        {
            Tile goTile = new Tile(positionInfo.PosX, positionInfo.PosY);
            return _tiles.GetValueOrDefault(goTile, false);
        }
    }
}