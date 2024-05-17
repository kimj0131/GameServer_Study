namespace ServerCore
{
    internal class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Hello Thread!");
            }
        }
        static void Main(string[] args)
        {
            // 쓰레드 생성 및 함수 연결 / 스레드는 기본적으로 foreground쓰레드로 만들어짐
            //Thread t = new Thread(MainThread);
            //t.Name = "Test Thread";
            // 백그라운드 쓰레드로 지정할 수 있음
            // Main이 종료되면 쓰레드가 실행중인 여부와 상관없이 종료된다
            //t.IsBackground = true;
            //t.Start();

            //Console.WriteLine("Waitng for Thread!");

            // Join을 하면 백그라운드 쓰레드가 종료될때 까지 기다린다.
            //t.Join();
            //Console.WriteLine("Hello, World!");

            // C#에서 지원하는 쓰레드풀링을 활용 >> 백그라운드에서 돌아가는것으로 추정
            // 직접 생성해서 쓰기보다 쓰레드풀링을 활용하는 것이 좋을 것
            // 최대로 동시에 돌릴수 있는 쓰레드를 자동으로 제한하기 때문에 직접 생성하는것에 대비 
            //ThreadPool.QueueUserWorkItem(MainThread);

            // SetMinThreads(최소 스레드, ?)
            ThreadPool.SetMinThreads(1, 1);
            // SetMinThreads(최대 스레드, ?)
            ThreadPool.SetMaxThreads(5, 5);

            //for (int i = 0; i < 4; i++)
            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) ; });

            // task를 이용하면 작업단위로 스케줄링
            // TaskCreationOptions.LongRunning : 다른 스레드 작업과는 다르게 오래걸리는 작업이 될것임을 알려줌 (워크 쓰레드 풀과 별도로 처리해준다)
            // 오래 사용할 것이지에 따라서 쓰레드풀을 조금더 효율적으로 관리할 수 있다
            for (int i = 0; i < 5; i++)
            {
                Task t = new Task(() => { while (true) ; });
                t.Start();
            }


            ThreadPool.QueueUserWorkItem(MainThread);
            while (true)
            {

            }

        }
    }
}
