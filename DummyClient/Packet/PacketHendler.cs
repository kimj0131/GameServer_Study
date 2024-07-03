using DummyClient;
using ServerCore;

// 새로운 패킷양식을 추가하더라도 핸들러에 작성하면 처리되도록 자동화
class PacketHandler
{
    public static void S_ChatHandler(PacketSession session, IPacket packet)
    {
        S_Chat chatPacket = packet as S_Chat;
        ServerSession serverSession = session as ServerSession;

        //if (chatPacket.playerId == 1)
        //Console.WriteLine(chatPacket.chat);
    }
}
