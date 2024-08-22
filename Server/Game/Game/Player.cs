using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Player : GameObject
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }

        public PlayerInfo PlayerInfo
        {
            get
            {
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo.PlayerId = PlayerId;
                playerInfo.ObjectInfo = BaseInfo;
                playerInfo.Name = Name;
                return playerInfo;
            }
        }
        
        public Player()
        {
            ObjectType = GameObjectType.Player;
        }
    }
}