using System.Collections.Generic;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class ObjectManager
    {
        public static ObjectManager Instance { get; } = new ObjectManager();
        private object _lock = new object();
        private Dictionary<int, GameObject> _gameObjects = new Dictionary<int, GameObject>();
        private int _objectId = 1;

        public GameObject Add(GameObjectType type)
        {
            GameObject gameObject = new GameObject();

            lock (_lock)
            {
                gameObject.BaseInfo.ObjectId = _objectId;
                gameObject.BaseInfo.ObjectType = type;
                
                _gameObjects.Add(_objectId, gameObject);
                _objectId++;
            }

            return gameObject;
        }
        
        public bool Remove(int objectId)
        {
            lock (_lock)
            {
                return _gameObjects.Remove(objectId);
            }
        }

        public GameObject Find(int roomId)
        {
            lock (_lock)
            {
                GameObject gameObject = null;
                if (_gameObjects.TryGetValue(roomId, out gameObject))
                    return gameObject;

                return null;
            }
        }
    }
}