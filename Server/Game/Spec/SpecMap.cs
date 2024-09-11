using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Google.Protobuf.Protocol;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;

namespace Server.Game.Spec
{
    public class SpecMap : ISpec<Tile>
    {
        private List<Tile> _tiles;
        private Dictionary<int, List<Tile>> _mapsByTile;
        private Dictionary<int, Map> _mapDictionary;
        private Dictionary<int, Grid> _gridDictionary;
        
        public int MaxX { get; } = 100;
        public int MaxY { get; }= 100;
        public int CellWidth { get; } = 1;

        public SpecMap(List<object> rawList)
        {
            Console.WriteLine("SpecMap생성!");
            _tiles = ConvertToList<Tile>(rawList);
            
            _mapsByTile = _tiles
                .GroupBy(tile => tile.MapId)
                .ToDictionary(tiles => tiles.Key, tiles => tiles.ToList());

            _mapDictionary = new Dictionary<int, Map>();
            foreach (var keyValue in _mapsByTile)
            {
                int mapId = keyValue.Key;
                List<Tile> tiles = keyValue.Value;
                Map map = new Map(mapId, tiles);
                _mapDictionary.Add(mapId, map);
            }

            _gridDictionary = new Dictionary<int, Grid>();
            foreach (var keyValue in _mapsByTile)
            {
                int mapId = keyValue.Key;
                
                GridSize mapSize = new GridSize(MaxX, MaxY);
                var distance = Distance.FromMeters(CellWidth);
                var cellSize = new Size(distance, distance);
                var traversalVelocity = Velocity.FromKilometersPerHour(4);
            
                Grid grid = Grid.CreateGridWithLateralAndDiagonalConnections(mapSize, cellSize, traversalVelocity);

                // 장해물 추가
                foreach (var tile in keyValue.Value)
                    grid.DisconnectNode(new GridPosition(tile.X, tile.Y));
                
                _gridDictionary.Add(mapId, grid);
            }
        }

        public Grid GetGrid(int mapId)
        {
            return _gridDictionary[mapId];
        }

        public bool CantGo(int mapId, PositionInfo positionInfo)
        {
            Map map = _mapDictionary[mapId];
            return map.IsWall(positionInfo);
        }

        public bool IsOutMap(PositionInfo position)
        {
            if (position.PosX < 0 || (int)position.PosX >= MaxX)
                return true;
            
            if (position.PosY < 0 || (int)position.PosY >= MaxY)
                return true;

            return false;
        }
        
        private List<T> ConvertToList<T>(List<object> objectList) where T : class
        {
            return objectList.OfType<T>().ToList();
        }
    }
    
    
    // MODEL
    public class Tile
    {
        public int TileId { get; set; }
        public int MapId { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
    
    public class Map
    {
        public int _mapId;
        private Dictionary<Wall, bool> _walls = new Dictionary<Wall, bool>();

        public Map(int mapId, List<Tile> tiles)
        {
            _mapId = mapId;
            tiles.ForEach(tile => _walls.Add(new Wall(tile.MapId, tile.X, tile.Y), true));
        }

        public bool IsWall(PositionInfo positionInfo)
        {
            Wall wall = new Wall(_mapId, (short)positionInfo.PosX, (short)positionInfo.PosY);
            return _walls.GetValueOrDefault(wall, false);
        }
    }

    public struct Wall
    {
        public int MapId { get; set; }
        public short X { get; set; }
        public short Y { get; set; }

        public Wall(int mapId, int x, int y)
        {
            MapId = mapId;
            X =(short)x;
            Y = (short)y;
        }
    }
}