﻿using ServerCore;
using System.Net;

namespace Server
{

    internal class Program
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void Main(string[] args)
        {

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry iPHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = iPHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기 listenSocket
            // 손님을 입장시킨다 > Init에서 OnAcceptCompleted이벤트를 통해 접속한다
            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            while (true)
            {
                ;
            }

        }
    }
}
