namespace ServerCore
{
    class Lock
    {
        // true 열린상태 false 닫힌상태
        // 커널단에서는 bool과 비슷한 역할을 한다
        ManualResetEvent _available = new ManualResetEvent(true);

        // 획득
        public void Acquire()
        {
            // ManualResetEvent는 동작을 나눠서 처리해 lock을 구현할 때 문제가 발생한다
            _available.WaitOne(); // 입장 시도
            _available.Reset(); // 문을 닫는다
        }

        // 반환
        public void Release()
        {
            _available.Set();   // 문을 연다
        }
    }

    internal class Program
    {
        static int _num = 0;

        static Lock _lock = new Lock();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}
