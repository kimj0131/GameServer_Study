namespace ServerCore
{
    internal class Program
    {
        // 경합조건 (Race Condition) : 두 개 이상의 프로세스가 공통 자원을 병행적으로 읽거나 쓰는 동작을 할 때,
        // 공용 데이터에 대한 접근이 어떤 순서에 따라 이루어졌는지에 따라 그 실행 결과가 같지않고 달라지는 상황

        static int number = 0;

        static void Thread_1()
        {
            // atomic = 원자성
            // 어떠한 동작은 한번에 일어나야 한다

            for (int i = 0; i < 100000; i++)
            {
                //number++;

                // 3단계를 걸쳐 연산이 진행되기 때문에 문제가 발생
                //int temp = number;
                //temp += 1;
                //number = temp;

                // Interlocked > 원자성을 보장하게 한다
                // 성능에서는 매우 큰 손해를 보게된다
                Interlocked.Increment(ref number);
            }

        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                //number--;

                Interlocked.Decrement(ref number);
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}
