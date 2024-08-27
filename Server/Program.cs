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

        static void BaseTick()
        {
            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.BaseTickUpdate);
            JobTimer.Instance.Push(BaseTick, 100);
        }
        
        static void SpawnMonsterTick()
        {
            GameRoom gameRoom = RoomManager.Instance.Find(1);
            gameRoom.Push(gameRoom.MonsterSpawnTickUpdate);
            JobTimer.Instance.Push(SpawnMonsterTick, 5000);
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
            
            // GAME TICK
            JobTimer.Instance.Push(BaseTick);
            JobTimer.Instance.Push(SpawnMonsterTick);

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