using System;
using System.Net;
using System.Threading;
using Server.Game;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();

        static void FlushRoom()
        {
            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.TickUpdate);
            JobTimer.Instance.Push(FlushRoom, 100);
        }
        
        static void MonsterTimer()
        {
            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.TickUpdate);
            JobTimer.Instance.Push(MonsterTimer, 5000);
        }

        static void Main(string[] args)
        {
            RoomManager.Instance.Add();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");
            
            JobTimer.Instance.Push(FlushRoom);

            while (true)
            {
                JobTimer.Instance.Flush();
                // GameRoom gameRoom = RoomManager.Instance.Find(1);
                // gameRoom.Push(gameRoom.TickUpdate);
                Thread.Sleep(100);
            }
        }
    }
}