using Server;
using ServerCore;

// 새로운 패킷양식을 추가하더라도 핸들러에 작성하면 처리되도록 자동화
class PacketHandler
{
    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        // Broadcast하는 행동 자체를 Action으로 정의 후 Job Queue에 넣어주는 방식으로 변경
        GameRoom room = clientSession.Room;
        room.Push(
            () => room.Broadcast(clientSession, chatPacket.chat)
        );
    }
}
