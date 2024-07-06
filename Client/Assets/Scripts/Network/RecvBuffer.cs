using System;

namespace ServerCore
{
    public class RecvBuffer
    {
        // readbuffer 
        // [r] [ ] [ ] [ ] [ ] [w] [ ] [ ] [ ] [ ] 
        ArraySegment<byte> _buffer;
        int _readPos;   // 현재 read중인 index
        int _writePos;  // 현재 write중인 index

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        // 유효 범위 >> 데이터가 얼마나 쌓여있는지 >> 아직 처리되지 않는 데이터의 크기
        public int DataSize { get { return _writePos - _readPos; } }
        // 버퍼의 유효범위를 뺀 나머지 >> 버퍼에 비어있는 공간 크기
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        // 유효 범위의 세그먼트 >> 어디서부터 데이터를 읽으면 되는지를 컨텐츠단에 요청
        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        // 수신할때 어디서부터 어디까지가 유효 범위인지를 넘겨줌
        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            int dataSize = DataSize;
            if (dataSize == 0)
            {
                // 남은 데이터가 없으면 복사하지 않고 커서 위치만 리셋
                _readPos = _writePos = 0;
            }
            else
            {
                // 남은 데이터가 있으면 시작위치로 복사
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        // 성공적으로 데이터를 가공해서 처리를 완료하면 커서위치를 이동
        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > DataSize)
                return false;

            _readPos += numOfBytes;
            return true;
        }

        // 클라이언트에서 데이터를 보낸 상황에서 수신했을때 write커서를 데이터 크기만큼 이동
        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize)
                return false;

            _writePos += numOfBytes;
            return true;
        }
    }
}
