namespace ServerCore
{
    internal class Program
    {
        // ** 별도의 조치없이 컴파일시 CPU가 성능을 좋게 하기위해 최적화를 거쳐 순서에 맞지 않게 실행시키는 결과가 나오므로
        // ** 예상했던 무한루프결과가 아닌 반복을 빠져나오는 결과가 나오게된다.
        // ** 이런현상을 해결하고 코드의 흐름을 의도한대로 조절하기위해 메모리 베리어를 사용한다

        // 메모리 베리어
        // A) 코드 재배치 억제
        // B) 가시성도 해결가능

        // 종류
        // 1) Full Memory Barrier (ASM MFENCE, C# Thread.MemoryBarrier) : Store/Load 둘다 막는다.
        // 2) Store Memory Barrier (ASM SFENCE) : Store만 막는다
        // 2) Load Memory Barrier (ASM LFENCE) : Load만 막는다

        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1; // Store y 메모리에 적재

            Thread.MemoryBarrier();

            r1 = x; // Load x 메모리에서 가져옴
        }

        static void Thread_2()
        {
            x = 1; // Store x

            Thread.MemoryBarrier();

            r2 = y; // Load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }

            // 

            Console.WriteLine($"{count}번만에 빠져나옴");
        }
    }
}
