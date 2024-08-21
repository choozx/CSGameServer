using System.Collections.Generic;

namespace Server.Game
{
    public class MonsterManager
    {
        public static MonsterManager Instance { get; } = new MonsterManager();
        private object _lock = new object();
        private Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        private int _objectId = 1;

        public void Update(GameRoom room)
        {
            
        }

        private Monster Add()
        {
            Monster monster = new Monster();

            lock (_lock)
            {
                
            }

            return monster;
        }
    }
}