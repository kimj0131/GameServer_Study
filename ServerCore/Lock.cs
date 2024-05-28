namespace ServerCore
{
    // ReaderWriteLock 구현해보기

    // 재귀적 락을 허용할 것인지 (No)
    // 스핀락 정책 (5000번 -> Yield)
    internal class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        // [Unuesed(1)] [WriteThreadId(15)] [ReadCount(16)]
        // WriteThreadId > WriteLock을 획득한 Thread의 ID
        // ReadCount > ReadLock을 획득했을때 여러 쓰레드들이 동시에 잡았을때 의 카운트
        int _flag = EMPTY_FLAG;

        public void WriteLock()
        {
            // 아무도 WriteLock or ReadLock을 획득하고 있지 않을 때, 경합해서 소유권을 얻는다.
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // 시도를 해서 성공하면 return
                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                        return;
                }
                Thread.Yield();
            }
        }
        public void WriteUnlock()
        {
            Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 아무도 WriteLock을 획득하고 있지 않으면, ReadCount를 1 늘린다
            while (true)
            {
                for (int i = 0; i <= MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);

                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                        return;

                }
                Thread.Yield();
            }
        }
        public void ReadUnlock()
        {
            Interlocked.Decrement(ref _flag);
        }
    }
}
