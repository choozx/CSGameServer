using System;
using System.Numerics;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using Server;
using Server.Game;
using Server.Game.Spec;

class PacketHandler
{
	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;
		
		if (clientSession.MyPlayer == null)
			return;
		if (clientSession.MyPlayer.Room == null)
			return;
		
		PlayerInfo info = clientSession.MyPlayer.PlayerInfo;
		
		// 검증
		SpecMap specMap = SpecManager.GetSpec<SpecMap>(typeof(SpecMap));
		if (specMap.IsOutMap(info.ObjectInfo.PosInfo))
		{
			// Init position
			info.ObjectInfo.PosInfo.PosX = 50;
			info.ObjectInfo.PosInfo.PosY = 77;
		}
		
		if (specMap.CantGo(1, movePacket.PosInfo))
			return;
		
		// 일단 서버에서 좌표 이동
		info.ObjectInfo.PosInfo = movePacket.PosInfo;
		
		// 다른 플레이어한테도 알려준다
		S_Move resMovePacket = new S_Move();
		resMovePacket.ObjectId = clientSession.MyPlayer.BaseInfo.ObjectId;
		resMovePacket.PosInfo = info.ObjectInfo.PosInfo;
		
		clientSession.MyPlayer.Room.Broadcast(resMovePacket);
	}

	public static void C_AttackHandler(PacketSession session, IMessage packet)
	{
		C_Attack attackPacket = packet as C_Attack;
		ClientSession clientSession = session as ClientSession;
		
		if (clientSession.MyPlayer == null)
			return;
		if (clientSession.MyPlayer.Room == null)
			return;

		Vector2 normalizeVector = new Vector2(attackPacket.NormalizeVector.NormalPosX, attackPacket.NormalizeVector.NormalPosY);
		Projectile projectile = ProjectileManager.Instance.Add(clientSession.MyPlayer, normalizeVector);
		
		AttackInfo attackInfo = new AttackInfo();
		
		// TODO 총알뿐만 아니라 다른 무기도 처리해야됨
		attackInfo.ObjectInfo = projectile.BaseInfo;
		attackInfo.ProjectileType = ProjectileType.Bullet;
		attackInfo.Damage = 10;
		
		S_Attack resAttackPacket = new S_Attack();
		resAttackPacket.ObjectId = clientSession.MyPlayer.BaseInfo.ObjectId;
		resAttackPacket.AttackInfo = attackInfo;
		
		clientSession.MyPlayer.Room.Broadcast(resAttackPacket);
	}

	public static void C_HitHandler(PacketSession session, IMessage packet)
	{
		C_Hit hitPacket = packet as C_Hit;
		ClientSession clientSession = session as ClientSession;

		ProjectileManager.Instance.Remove(hitPacket.ProjectileObjectId);
		MonsterManager.Instance.Remove(hitPacket.MonsterObjectId);

		S_Despawn resHitPacket = new S_Despawn();
		resHitPacket.ObjectIds.Add(hitPacket.MonsterObjectId);
		resHitPacket.ObjectIds.Add(hitPacket.ProjectileObjectId);
		clientSession.MyPlayer.Room.Broadcast(resHitPacket);
	}
}
