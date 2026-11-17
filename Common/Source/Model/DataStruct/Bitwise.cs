using Common.Source.Extension;
using System.Numerics;

namespace Common.Source.Model.DataStruct
{
    public struct Bitwise<T> where T : struct, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
    {
        public T Bits { get; private set; }

        public Bitwise(T bits)
        {
            Bits = bits;
        }

        public T Pop(int length = 1)
        {
            return Peek(length).Configure(Bits = Bits >>> length);
        }

        public T HighPop(int length = 1)
        {
            return HighPeek(length).Configure(Bits <<= length);
        }

        public T Push(T value, int length = 1)
        {
            return Bits = Bits << length | value;
        }

        public T HighPush(T value, int length = 1)
        {
            return Bits = Bits >>> length | value << TypeSize<T>.BitSize - length;
        }

        public readonly T Peek(int length = 1)
        {
            return Bits & GetMask(length);
        }

        public readonly T Peek(int length, int offset)
        {
            return Bits & GetMask(length, offset);
        }

        public readonly T HighPeek(int length = 1)
        {
            return Bits >>> TypeSize<T>.BitSize - length;
        }

        public readonly T HighPeek(int length, int offset)
        {
            return Bits << offset >>> TypeSize<T>.BitSize - length;
        }

        public static T GetMask(int length)
        {
            return ~(~default(T) << length);
        }

        public static T GetMask(int length, int offset)
        {
            return GetMask(length) << offset;
        }

        public static T GetHighMask(int length)
        {
            return ~(~default(T) >>> length);
        }

        public static T GetHighMask(int length, int offset)
        {
            return GetHighMask(length) >>> offset;
        }
    }
}