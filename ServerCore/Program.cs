namespace ServerCore
{
    // [ JobQueue ] 
    // 모든쓰레드들이 접근할 때 락을 이용해서 풀었다 잠궜다 하기보다는 
    // TLS공간에 여러개의 일감을 뽑아와 활용하면 된다
    // 대량의 일감을 처리하기 전에 JobQueue에 접근할 필요성이 없어진다
    // 공용 공간에 접근하는 횟수를 줄이는데 의미가 있다고 할 수 있다

    internal class Program
    {
        // TLS 실습

        // static 전역메모리, 모든 Thread가 공유해서 사용한다
        // TLS 영역으로 두고 싶다면 ThreadLocal로 wrapping한다
        // Thread 마다 접근하면 자신의 공간에 저장이되기 때문에 특정 Thread에서 수정을 하더라도 다른 Thread에서는 영향을 주지 않는다
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });
        //static string ThreadName;

        static void WhoAmI()
        {
            //ThreadName.Value = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";
            //ThreadName = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";

            bool repeat = ThreadName.IsValueCreated;

            if (repeat)
                Console.WriteLine(ThreadName.Value + "Repeat");
            else
                Console.WriteLine(ThreadName.Value);

            //Console.WriteLine(ThreadName);
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            // 필요없어질 경우 Dispose를 이용해 제거한다
            ThreadName.Dispose();
        }
    }
}
