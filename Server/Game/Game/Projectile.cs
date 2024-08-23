using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Projectile : GameObject
    {
        public int projectileId { get; }
        public Vector2 _normalizeVector { get; set; }
        public Vector2 _initPosition { get; set; }
        // TODO 이동 거리 추가

        public Projectile()
        {
            ObjectType = GameObjectType.Projectile;
        }

        public virtual void Update()
        {
            
        }

        public void Release()
        {
            ProjectileManager.Instance.Remove(BaseInfo.ObjectId);

            S_Despawn despawnPacket = new S_Despawn();
            despawnPacket.ObjectIds.Add(BaseInfo.ObjectId);
            Room.Broadcast(despawnPacket);
        }
    }
}