namespace ServerCore
{
    internal class Program
    {
        static void MainThread()
        {
            while (true)
                Console.WriteLine("Hello Thread!");
        }
        static void Main(string[] args)
        {
            // 쓰레드 생성 및 함수 연결 / 스레드는 기본적으로 foreground쓰레드로 만들어짐
            Thread t = new Thread(MainThread);
            // 백그라운드 쓰레드로 지정할 수 있음
            // Main이 종료되면 쓰레드가 실행중인 여부와 상관없이 종료된다
            t.IsBackground = true;
            t.Start();
            Console.WriteLine("Waitng for Thread!");
            // Join을 하면 백그라운드 쓰레드가 종료될때 까지 기다린다.
            t.Join();
            Console.WriteLine("Hello, World!");
        }
    }
}
