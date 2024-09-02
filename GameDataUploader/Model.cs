using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataUploader
{
    
    public class Wall
    {
        public int WallId { get; set; }
        public int MapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}