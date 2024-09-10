using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataUploader
{
    
    public class Tile
    {
        public int TileId { get; set; }
        public int MapId { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
}