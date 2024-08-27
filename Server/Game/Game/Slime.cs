using System;
using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Slime : Monster
    {
        
        public Slime()
        {
            _hp = 100;
            _speed = 2.0f;
            _monsterType = MonsterType.Slime;
            _searchDistance = 50.0f;
            _chaseDistance = 100.0f;
        }

        protected override void Move()
        {
            Vector2 playerVector = PlayerManager.Instance.GetPlayerPosition(_targetPlayerId);
            Vector2 monsterVector = new Vector2(BaseInfo.PosInfo.PosX, BaseInfo.PosInfo.PosY);

            Vector2 calcVector = playerVector - monsterVector;
            if (calcVector.Length() > _chaseDistance)
            {
                _targetPlayerId = 0;
                return;
            }
            
            Vector2 normalizeVector = Vector2.Normalize(calcVector);
            BaseInfo.PosInfo.PosX += normalizeVector.X * _speed;
            BaseInfo.PosInfo.PosY += normalizeVector.Y * _speed;
            
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = BaseInfo.ObjectId;
            movePacket.PosInfo = BaseInfo.PosInfo;
            Room.Broadcast(movePacket);
        }
    }
}