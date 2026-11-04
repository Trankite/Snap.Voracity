namespace Common.Source.Model.DataStruct
{
    public class BitSet
    {
        public readonly int Count;

        public readonly byte[] Bytes;

        private BitSet(int count, byte[] bytes)
        {
            Count = count;
            Bytes = bytes;
        }

        public static BitSet Create(int count)
        {
            return new BitSet(count, new byte[(count + 7) >> 3]);
        }

        public static BitSet Create(byte[] bytes)
        {
            return new BitSet(bytes.Length * 8, bytes);
        }

        public bool this[int i]
        {
            get => (Bytes[i >> 3] & (1 << (7 - (i & 7)))) != 0;
            set
            {
                if (value)
                {
                    Bytes[i >> 3] |= (byte)(1 << (7 - (i & 7)));
                }
                else
                {
                    Bytes[i >> 3] &= (byte)~(1 << (7 - (i & 7)));
                }
            }
        }

        public void Write(int destinationIndex, int source, int sourceBitCount)
        {
            for (int i = 0; i < sourceBitCount; i++)
            {
                this[destinationIndex + i] = (source & (1 << (sourceBitCount - 1 - i))) != 0;
            }
        }

        public void Write(int destinationIndex, BitSet source, int sourceIndex, int sourceCount)
        {
            for (int i = 0; i < sourceCount; i++)
            {
                this[destinationIndex + i] = source[sourceIndex + i];
            }
        }
    }
}