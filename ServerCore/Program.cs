namespace ServerCore
{
    internal class Program
    {
        // 경합조건 (Race Condition) : 두 개 이상의 프로세스가 공통 자원을 병행적으로 읽거나 쓰는 동작을 할 때,
        // 공용 데이터에 대한 접근이 어떤 순서에 따라 이루어졌는지에 따라 그 실행 결과가 같지않고 달라지는 상황

        static int number = 0;

        static object _obj = new object();

        static void Thread_1()
        {
            // atomic = 원자성
            // 어떠한 동작은 한번에 일어나야 한다

            for (int i = 0; i < 100000; i++)
            {
                // 상호배제 Mutual Exclusive
                // 잠금을 풀기전까지 number을 다른 쓰레드에서 건드릴 수 없음
                // 유지보수가 힘들어진다
                // try finally문을 사용하면 Exit하지 못하는경우를 방지할 수는 있음

                //Monitor.Enter(_obj);    // 문을 잠구는 행위
                //{
                //    number++;
                //    //예시 > 이러는 경우 
                //    //return;
                //}
                //Monitor.Exit(_obj);     // 잠금을 푼다

                // 위의 번거로운 부분을 lock을 사용하면 쉽게 구현할 수 있음
                lock (_obj)
                {
                    number++;
                }

            }

        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                lock (_obj)
                {
                    number++;
                }
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
