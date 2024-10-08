using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        object _lock = new object();
        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        int _playerId = 1; // TODO
		
        public Player Add()
        {
            Player player = new Player();

            lock (_lock)
            {
                player.BaseInfo = ObjectManager.Instance.Add(GameObjectType.Player).BaseInfo;
                player.PlayerId = _playerId;
                
                _players.Add(_playerId, player);
                _playerId++;
            }

            return player;
        }

        public bool Remove(int playerId)
        {
            lock (_lock)
            {
                return _players.Remove(playerId);
            }
        }

        public Player Find(int playerId)
        {
            lock (_lock)
            {
                Player player = null;
                if (_players.TryGetValue(playerId, out player))
                    return player;

                return null;
            }
        }

        public Dictionary<int, Player> GetPlayerDictionary()
        {
            return _players;
        }

        public Vector2 GetPlayerPosition(int playerId)
        {
            Player player = _players[playerId];

            PositionInfo posInfo = player.BaseInfo.PosInfo;
            Vector2 position = new Vector2(posInfo.PosX, posInfo.PosY);
            return position;
        }
    }
}