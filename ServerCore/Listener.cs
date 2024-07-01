using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            // 문지기 교육 ipAddress 연동
            _listenSocket.Bind(endPoint);

            // 영업시작 Listen
            // backlog : 최대 대기수
            _listenSocket.Listen(backlog);


            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }

        // 접속 예약?
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // SocketAsyncEventArgs를 비워줘야 다음 접속자의 정보를 불러올 수 있음
            args.AcceptSocket = null;

            // 계류 중인지를 반환한다 >> false일때 접속이 되었다는 의미?
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        // Accept가 성공했을때 실행되는 로직
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            // 접속을 위한 일이 끝났으므로 다음 접속자를 위해 등록
            RegisterAccept(args);
        }

    }
}
