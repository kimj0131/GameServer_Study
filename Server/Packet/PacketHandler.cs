using ServerCore;

namespace Server
{
    // 새로운 패킷양식을 추가하더라도 핸들러에 작성하면 처리되도록 자동화
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            Console.WriteLine($"PlayerInfoReq : {p.playerId} {p.name}");

            foreach (PlayerInfoReq.Skill skill in p.skills)
            {
                Console.WriteLine($"Skill[{skill.id}][{skill.level}][{skill.duration}]");
            }
        }
    }
}
