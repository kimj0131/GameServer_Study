namespace Server
{
    class GameRoom
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        // ** 같은 세션에 있는 사용자들한테 순회하며 메시지를 뿌리는 작업
        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new S_Chat();
            packet.playerId = session.SessionId;
            packet.chat = $"{chat} I am {packet.playerId}";
            ArraySegment<byte> segment = packet.Write();

            // @@ 규모가 커지면 모든 작업쓰레드들이 lock에서 멈추고,
            // 이로인해 새로운 쓰레드를 계속 생성하는 문제가 발생 
            // 한 쓰레드가 작업중이면 나머지 쓰레드는
            // 작업 Queue에 저장만하고 빠져나오는 방법으로 해결할 수 있다
            lock (_lock)
            {
                foreach (ClientSession s in _sessions)
                    s.Send(segment);
            }
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Add(session);
                session.Room = this;
            }
        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Remove(session);
            }
        }
    }
}
