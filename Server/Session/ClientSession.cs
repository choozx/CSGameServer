using System;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public Player MyPlayer { get; set; }
        public int SessionId { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            MyPlayer = PlayerManager.Instance.Add();
            {
                MyPlayer.Name = $"Player_{MyPlayer.PlayerId}";
                MyPlayer.BaseInfo.PosInfo.State = CreatureState.Idle;
                MyPlayer.BaseInfo.PosInfo.MoveDir = MoveDir.Down;
                MyPlayer.BaseInfo.PosInfo.PosX = 50;
                MyPlayer.BaseInfo.PosInfo.PosY = 77;

                MyPlayer.Session = this;
            }

            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.EnterGame, MyPlayer);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.LeaveGame, MyPlayer.PlayerId);
            
            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}