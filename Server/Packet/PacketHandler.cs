using ServerCore;

// 새로운 패킷양식을 추가하더라도 핸들러에 작성하면 처리되도록 자동화
class PacketHandler
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;

        Console.WriteLine($"PlayerInfoReq : {p.playerId} {p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"Skill[{skill.id}][{skill.level}][{skill.duration}]");
        }
    }

}
