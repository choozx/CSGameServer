using System;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Slime : Monster
    {
        public Slime()
        {
            _hp = 100;
            _monsterType = MonsterType.Slime;
        }

        public override void Update()
        {
            
        }
    }
}