using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Monster : GameObject
    {
        public int _hp;
        public float _speed;
        public MonsterType _monsterType;
        
        public int _targetPlayerId;
        public float _searchDistance;
        public float _chaseDistance;

        public Monster()
        {
            ObjectType = GameObjectType.Monster;
            _targetPlayerId = 0;
        }

        public void Update()
        {
            if (_targetPlayerId == 0)
            {
                // 범위안에 타겟 플레이어 찾기
                _targetPlayerId = MonsterManager.Instance.getPlayerIdByclosest(BaseInfo.PosInfo, _searchDistance);
            }
            else
            {
                Move();
                
            }
        }

        protected virtual void Move()
        {
            
        }
        
        public void Release()
        {
            ObjectManager.Instance.Remove(BaseInfo.ObjectId);
            MonsterManager.Instance.Remove(BaseInfo.ObjectId);

            S_Despawn despawnPacket = new S_Despawn();
            despawnPacket.ObjectIds.Add(BaseInfo.ObjectId);
            Room.Broadcast(despawnPacket);
        }
    }
}