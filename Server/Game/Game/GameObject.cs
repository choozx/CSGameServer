using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class GameObject
    {
        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;
        public GameRoom Room { get; set; }
        public ClientSession Session { get; set; }
        public ObjectInfo BaseInfo { get; set; } = new ObjectInfo() {PosInfo = new PositionInfo()};
    }
}