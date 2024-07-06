using ServerCore;
using System;
using System.Net;

namespace DummyClient
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            // Packet Queue에 넣어두라는 작업만하게 처리
            PacketManager.Instance.OnRecvPacket(this, buffer, (session, packet) => PacketQueue.Instance.Push(packet));
        }

        public override void OnSend(int numOfByte)
        {
            //Console.WriteLine($"Transferred bytes : {numOfByte}");
        }
    }

}
