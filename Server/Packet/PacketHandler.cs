using System;
using System.Numerics;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using Server;
using Server.Game;

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
		
		// TODO : 검증
		
		// 일단 서버에서 좌표 이동
		PlayerInfo info = clientSession.MyPlayer.PlayerInfo;
		info.ObjectInfo.PosInfo = movePacket.PosInfo;
		
		// 다른 플레이어한테도 알려준다
		S_Move resMovePacket = new S_Move();
		resMovePacket.ObjectId = clientSession.MyPlayer.BaseInfo.ObjectId;
		resMovePacket.PosInfo = movePacket.PosInfo;
		
		Console.WriteLine($"{clientSession.MyPlayer.PlayerId} | {resMovePacket}");
		
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
		
		attackInfo.ObjectInfo = projectile.BaseInfo;
		attackInfo.ProjectileType = ProjectileType.Bullet;
		attackInfo.Damage = 10;
		
		S_Attack resAttackPacket = new S_Attack();
		resAttackPacket.ObjectId = clientSession.MyPlayer.BaseInfo.ObjectId;
		resAttackPacket.AttackInfo = attackInfo;
		
		clientSession.MyPlayer.Room.Broadcast(resAttackPacket);
	}
}
