using System;
using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class MonsterManager
    {
        public static MonsterManager Instance { get; } = new MonsterManager();
        private object _lock = new object();
        private Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        private int _maxMonsterCount = 1;

        public void SpawnUpdate(GameRoom room)
        {
            lock (_lock)
            {
                // spawn
                if (_monsters.Count != _maxMonsterCount)
                {
                    int todoSpawnMonster = _maxMonsterCount - _monsters.Count;
                    
                    // TODO 추후에는 어떤 몬스터를 생성할지에 대한 로직 필요 (현재는 슬라임만 소환)
                    
                    S_MonsterSpawn resMonsterSpawn = new S_MonsterSpawn();
                    for (int i = 0; i < todoSpawnMonster; i++)
                    {
                        Monster monster = Add(room);

                        MonsterInfo monsterInfo = new MonsterInfo();
                        monsterInfo.ObjectInfo = monster.BaseInfo;
                        monsterInfo.MonsterType = monster.MonsterType;
                        resMonsterSpawn.Monsters.Add(monsterInfo);
                        Console.WriteLine($"몬스터 소환! : {monsterInfo.MonsterType}_{monsterInfo.ObjectInfo.ObjectId}");
                    }
                    
                    room.Broadcast(resMonsterSpawn);
                }
            }
        }

        public void BaseUpdate()
        {
            lock (_lock)
            {
                foreach (var monster in _monsters.Values)
                {
                    monster.Update();
                }   
            }
        }

        public List<MonsterInfo> GetMonsterInfoList()
        {
            List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
            foreach (var monster in _monsters.Values)
            {
                MonsterInfo monsterInfo = new MonsterInfo();
                monsterInfo.MonsterType = monster.MonsterType;
                monsterInfo.ObjectInfo = monster.BaseInfo;
                
                monsterInfos.Add(monsterInfo);
            }
            return monsterInfos;
        }
        
        public bool Remove(int objectId)
        {
            lock (_lock)
            {
                ObjectManager.Instance.Remove(objectId);
                return _monsters.Remove(objectId);
            }
        }
        
        public int getPlayerIdByclosest(PositionInfo monsterPosInfo, float searchDistance)
        {
            int playerId = 0;
            Vector2 otherVector = new Vector2(monsterPosInfo.PosY, monsterPosInfo.PosX);

            Dictionary<int, Player> players = PlayerManager.Instance.GetPlayerDictionary();
            foreach (var player in players)
            {
                PositionInfo playerPosInfo = player.Value.BaseInfo.PosInfo;

                Vector2 playerVector = new Vector2(playerPosInfo.PosY, playerPosInfo.PosX);

                float distance = Vector2.Distance(otherVector, playerVector);

                if (searchDistance >= distance)
                    playerId = player.Key;
            }
            return playerId;
        }
        
        private Monster Add(GameRoom room)
        {
            Monster monster = new Slime();

            lock (_lock)
            {
                monster.BaseInfo = ObjectManager.Instance.Add(GameObjectType.Monster).BaseInfo;
                monster.Room = room;
                
                PositionInfo positionInfo = new PositionInfo();
                Random random = new Random();
                positionInfo.PosX = random.Next(1, 99);
                positionInfo.PosY = random.Next(1, 99);
                monster.BaseInfo.PosInfo = positionInfo;
                
                _monsters.Add(monster.BaseInfo.ObjectId, monster);
            }

            return monster;
        }
    }
}