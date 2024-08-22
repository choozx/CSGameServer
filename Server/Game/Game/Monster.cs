using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Monster : GameObject
    {
        public Monster()
        {
            ObjectType = GameObjectType.Monster;
        }

        public virtual void Update()
        {
            
        }
        
        public void Release()
        {
            ObjectManager.Instance.Remove(BaseInfo.ObjectId);
            MonsterManager.Instance.Remove(BaseInfo.ObjectId);

            S_Despawn despawnPacket = new S_Despawn();
            despawnPacket.ObjectIds.Add(BaseInfo.ObjectId);
            Room.Broadcast(despawnPacket);
        }
    }
}