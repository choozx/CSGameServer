using System.Data;
using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Bullet : Projectile
    {
        public GameObject Owner { get; set; }
        public float moveDistance { get; set; } = 0;
        
        private readonly float _speed = 50.0f;
        private float _maxDistance = 2000.0f;

        public override void Update()
        {
            if (moveDistance > _maxDistance)
                Release();
            
            BaseInfo.PosInfo.PosX += _normalizeVector.X * _speed;
            BaseInfo.PosInfo.PosY += _normalizeVector.Y * _speed;
            
            // 이동거리 계산
            Vector2 newPosition = new Vector2(BaseInfo.PosInfo.PosX, BaseInfo.PosInfo.PosY);
            newPosition -= _initPosition;
            moveDistance = newPosition.Length();
            
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = BaseInfo.ObjectId;
            movePacket.PosInfo = BaseInfo.PosInfo;
            Room.Broadcast(movePacket);
        }
    }
}