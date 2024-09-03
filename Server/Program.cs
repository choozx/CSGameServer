using System;
using System.Net;
using System.Threading;
using Server.Config;
using Server.Game;
using Server.Game.Spec;
using ServerCore;
using Tile = Server.Game.Tile;

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
            SpecDBContext.InitializeDB();
            GameDBContext.InitializeDB();
            Console.WriteLine("Init DB");
            
            // 게임 데이터 리로드
            SpecManager.Instance.LoadAll();
            
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
                Thread.Sleep(100);
            }
        }
    }
}