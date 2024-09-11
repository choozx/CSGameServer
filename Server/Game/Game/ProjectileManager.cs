using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class ProjectileManager
    {
        public static ProjectileManager Instance { get; } = new ProjectileManager();
        private object _lock = new object();
        private Dictionary<int, Projectile> _projectiles = new Dictionary<int, Projectile>();
        
        public Projectile Add(Player player, Vector2 normalizeVector)
        {
            // TODO 팩토리 패턴으로 객체 생성
            Projectile projectile = new Bullet();

            lock (_lock)
            {
                projectile.BaseInfo = ObjectManager.Instance.Add(GameObjectType.Projectile).BaseInfo;
                projectile.Room = player.Room;
                
                PositionInfo positionInfo = new PositionInfo(player.BaseInfo.PosInfo);
                projectile.BaseInfo.PosInfo = positionInfo;
                projectile._normalizeVector = normalizeVector;
                projectile._initPosition = new Vector2(positionInfo.PosX, positionInfo.PosY);
                
                _projectiles.Add(projectile.BaseInfo.ObjectId, projectile);   
            }

            return projectile;
        }

        public void Update()
        {
            lock (_lock)
            {
                foreach (var projectile in _projectiles.Values)
                {
                    projectile.Update();
                }
            }
        }

        public bool Remove(int objectId)
        {
            lock (_lock)
            {
                ObjectManager.Instance.Remove(objectId);
                return _projectiles.Remove(objectId);
            }
        }
    }
}