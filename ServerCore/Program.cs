namespace ServerCore
{
    internal class Program
    {
        // 전역변수는 모든 Thread가 동시에 접근할 수 있다
        // volatile : 휘발성데이터 선언(릴리즈 모드 컴파일시 최적화 하지않음)
        // C++와 동작이 다르기 때문에 혼동우려가 있어 잘 사용하지 않는다고 함
        volatile static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while (_stop == false)
            {
                // 누군가가 stop 신호를 해주기를 기다린다
            }

            Console.WriteLine("쓰레드 종료!");
        }

        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            // 1000밀리세컨드 만큼 대기를 한다
            // 1초의 대기시간 후에 _stop을 true로 전환
            Thread.Sleep(1000);
            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}
