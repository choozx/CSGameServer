using System;
using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Protocol;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using Server.Game.Algorithms;
using Server.Game.Spec;

namespace Server.Game
{
    public class Slime : Monster
    {
        
        public Slime()
        {
            Hp = 100;
            Speed = 4.0f;
            MonsterType = MonsterType.Slime;
            SearchDistance = 20.0f;
            ChaseDistance = 40.0f;

            // TODO Enum
            MapId = 1;
        }

        protected override void Move()
        {
            Vector2 playerVector = PlayerManager.Instance.GetPlayerPosition(TargetPlayerId);
            Vector2 monsterVector = new Vector2(BaseInfo.PosInfo.PosX, BaseInfo.PosInfo.PosY);

            // float distance = Vector2.Distance(monsterVector, playerVector);
            // if (distance > ChaseDistance)
            // {
            //     TargetPlayerId = 0;
            //     return;
            // }
            
            // TODO 왜 장해물에 나갔다 들어오면 다시 추격을 안하지?
            var destination = Navigation.FindPath(monsterVector, playerVector, MapId);
            if (destination == null)
            {
                TargetPlayerId = 0;
                return;
            }

            // Vector2 startPosition = ToVector(destination.Edges[0].Start.Position);
            Vector2 endPosition = ToVector(destination.End.Position);
            destination = null;
            // 속도에 따라 몇칸 이동했는지로 해야겠네
            
            // Vector2 normalizeVector = Vector2.Normalize(endPosition - startPosition);
            // BaseInfo.PosInfo.PosX = Math.Clamp(BaseInfo.PosInfo.PosX + (normalizeVector.X * Speed), 0, specMap.MaxX);
            // BaseInfo.PosInfo.PosY = Math.Clamp(BaseInfo.PosInfo.PosY + (normalizeVector.Y * Speed), 0, specMap.MaxY);
            
            BaseInfo.PosInfo.PosX = endPosition.X;
            BaseInfo.PosInfo.PosY = endPosition.Y;
            
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = BaseInfo.ObjectId;
            movePacket.PosInfo = BaseInfo.PosInfo;
            Room.Broadcast(movePacket);
        }

        private Vector2 ToVector(Position position)
        {
            return new Vector2(position.X, position.Y);
        }
    }
}