using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameRoom : JobSerializer
    {
        public int RoomId { get; set; }

        private Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public void EnterGame(Player newPlayer)
        {
            if (newPlayer == null)
                return;

            _players.Add(newPlayer.PlayerId, newPlayer);
            newPlayer.Room = this;

            // 본인한테 정보 전송
            {
                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.Player = newPlayer.PlayerInfo;
                newPlayer.Session.Send(enterPacket);

                S_Spawn spawnPacket = new S_Spawn();
                foreach (Player p in _players.Values)
                {
                    if (newPlayer != p)
                        spawnPacket.Players.Add(p.PlayerInfo);
                }
                newPlayer.Session.Send(spawnPacket);
            }

            // 타인한테 정보 전송
            {
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Players.Add(newPlayer.PlayerInfo);
                foreach (Player p in _players.Values)
                {
                    if (newPlayer != p)
                        p.Session.Send(spawnPacket);
                }
            }
            
            // 몬스터 정보 전송
            List<MonsterInfo> monsterInfos = MonsterManager.Instance.GetMonsterInfoDictionary();
            S_MonsterSpawn monsterSpawn = new S_MonsterSpawn();
            monsterSpawn.Monsters.AddRange(monsterInfos);
            newPlayer.Session.Send(monsterSpawn);
        }

        public void LeaveGame(int playerId)
        {
            Player player = _players[playerId];
            if (player == null)
                return;

            _players.Remove(playerId);
            player.Room = null;

            // 본인한테 정보 전송
            {
                S_LeaveGame leavePacket = new S_LeaveGame();
                player.Session.Send(leavePacket);
            }

            // 타인한테 정보 전송
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectIds.Add(player.PlayerId);
                foreach (Player p in _players.Values)
                {
                    if (player != p)
                        p.Session.Send(despawnPacket);
                }
            }
        }

        public void BaseTickUpdate()
        {
            ProjectileManager.Instance.Update();
        }

        public void MonsterTickUpdate()
        {
            MonsterManager.Instance.Update(this);
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player p in _players.Values)
            {
                p.Session.Send(packet);
            }
        }
    }
}