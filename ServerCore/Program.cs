namespace ServerCore
{
    class Lock
    {
        // true 열린상태 false 닫힌상태
        // 커널단에서는 bool과 비슷한 역할을 한다
        AutoResetEvent _available = new AutoResetEvent(true);

        // 획득
        public void Acquire()
        {
            // 입장 시도
            _available.WaitOne();
        }

        // 반환
        public void Release()
        {
            _available.Set();
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
