using System;
using System.Collections.Generic;
using System.Linq;
using Roy_T.AStar.Grids;

namespace Server.Game.Spec
{
    public class SpecMap : ISpec<Tile>
    {
        private List<Tile> _tiles;
        private Dictionary<int, Grid> _maps;

        public SpecMap(List<object> rawList)
        {
            _tiles = ConvertToList<Tile>(rawList);
            Console.WriteLine(rawList);
        }
        
        private List<T> ConvertToList<T>(List<object> objectList) where T : class
        {
            return objectList.OfType<T>().ToList();
        }
    }

    public class Tile
    {
        public int TileId { get; set; }
        public int MapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}