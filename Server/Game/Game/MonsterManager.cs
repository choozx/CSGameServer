using System;
using System.Collections.Generic;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class MonsterManager
    {
        public static MonsterManager Instance { get; } = new MonsterManager();
        private object _lock = new object();
        private Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        private int _maxMonsterCount = 10;
        private int _monsterId = 1;

        public void Update(GameRoom room)
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
                        monsterInfo.MonsterId = monster.BaseInfo.ObjectId;
                        monsterInfo.ObjectInfo = monster.BaseInfo;
                        monsterInfo.MonsterType = monster._monsterType;
                        resMonsterSpawn.Monsters.Add(monsterInfo);
                        Console.WriteLine($"몬스터 소환! : {monsterInfo.MonsterType}_{monsterInfo.MonsterId}");
                    }
                    
                    room.Broadcast(resMonsterSpawn);
                }
                
                // move and attack
                foreach (var monster in _monsters.Values)
                {
                    monster.Update();
                }
            }
        }

        public List<MonsterInfo> GetMonsterInfoDictionary()
        {
            List<MonsterInfo> monsterInfos = new List<MonsterInfo>();
            foreach (var monster in _monsters.Values)
            {
                MonsterInfo monsterInfo = new MonsterInfo();
                monsterInfo.MonsterId = monster._monsterId;
                monsterInfo.MonsterType = monster._monsterType;
                monsterInfo.ObjectInfo = monster.BaseInfo;
                
                monsterInfos.Add(monsterInfo);
            }
            return monsterInfos;
        }
        
        public bool Remove(int objectId)
        {
            lock (_lock)
            {
                return _monsters.Remove(objectId);
            }
        }
        
        private Monster Add(GameRoom room)
        {
            Monster monster = new Slime();

            lock (_lock)
            {
                monster.BaseInfo = ObjectManager.Instance.Add(GameObjectType.Monster).BaseInfo;
                monster._monsterId += _monsterId;
                monster.Room = room;
                
                PositionInfo positionInfo = new PositionInfo();
                Random random = new Random();
                int x = random.Next(100);
                int y = random.Next(100);
                positionInfo.PosX = x;
                positionInfo.PosY = y;
                monster.BaseInfo.PosInfo = positionInfo;
                
                _monsters.Add(monster.BaseInfo.ObjectId, monster);
                _monsterId++;
            }

            return monster;
        }
    }
}