using ServerCore;
using System.Net;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry iPHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = iPHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // Connector를 이용해 연결
            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });

            // 동일한 행동을 테스트를 하기 위해 반복 설정
            while (true)
            {
                try
                {

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                // 반복하는 딜레이 설정
                Thread.Sleep(1000);
            }

        }
    }
}
