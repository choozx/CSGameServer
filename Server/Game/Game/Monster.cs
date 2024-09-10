using System.Numerics;
using Google.Protobuf.Protocol;
using Roy_T.AStar.Grids;

namespace Server.Game
{
    public class Monster : GameObject
    {
        public int Hp;
        public float Speed;
        public MonsterType MonsterType;
        
        public int TargetPlayerId;
        public float SearchDistance;
        public float ChaseDistance;

        public int MapId;

        public Monster()
        {
            ObjectType = GameObjectType.Monster;
            TargetPlayerId = 0;
        }

        public void Update()
        {
            if (TargetPlayerId == 0)
            {
                // 범위안에 타겟 플레이어 찾기
                TargetPlayerId = MonsterManager.Instance.getPlayerIdByclosest(BaseInfo.PosInfo, SearchDistance);
            }
            else
            {
                Move();
            }
        }

        protected virtual void Move()
        {
            
        }
    }
}