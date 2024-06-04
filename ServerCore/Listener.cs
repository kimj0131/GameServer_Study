using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    internal class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHandler;

        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;

            // 문지기 교육 ipAddress 연동
            _listenSocket.Bind(endPoint);

            // 영업시작 Listen
            // backlog : 최대 대기수
            _listenSocket.Listen(10);

            // SocketAsyncEventArgs는 한번 생성하면 계속 재사용할 수 있다
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

            // 최초 한번은 직접등록하여 접속자를 기다린다
            RegisterAccept(args);
            // 클라이언트가 실제로 커넥트 요청이 오면 콜백방식으로 OnAcceptCompleted가 호출이 된다
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
                // TODO
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            // 접속을 위한 일이 끝났으므로 다음 접속자를 위해 등록
            RegisterAccept(args);
        }

    }
}
