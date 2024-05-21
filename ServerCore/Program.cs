namespace ServerCore
{
    class SpinLock
    {
        // 상태변수
        volatile int _locked = 0;

        // 획득
        public void Acquire()
        {
            //while (_locked)
            //{
            //    // 잠김이 풀리기를 기다린다
            //}
            //// 획득한다
            //_locked = true;

            // 잠기고 풀리는 과정이 한번에 일어나야함
            while (true)
            {
                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //    break;

                // 다른 일반적인 버전
                //CAS Compare-And-Swap 연산
                int expected = 0;   // 예상값
                int desired = 1;    // 원하는값
                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    break;
            }
        }

        // 반환
        public void Release()
        {
            _locked = 0;
        }
    }

    internal class Program
    {
        static int _num = 0;

        static SpinLock _lock = new SpinLock();

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
