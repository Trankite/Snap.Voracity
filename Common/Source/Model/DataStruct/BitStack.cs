using Common.Source.Extension;
using Common.Source.Service.Helper;
using System.Numerics;

namespace Common.Source.Model.DataStruct
{
    public struct BitStack<T> : IShiftOperators<BitStack<T>, int, BitStack<T>>, IBitwiseOperators<BitStack<T>, BitStack<T>, BitStack<T>> where T : struct, IShiftOperators<T, int, T>, IBitwiseOperators<T, T, T>
    {
        public T Bits { get; private set; }

        public BitStack(T bits)
        {
            Bits = bits;
        }

        public readonly T Peek(int length = 1)
        {
            return Bits & BitHelper.GetMask<T>(length);
        }

        public readonly T Peek(int length, int offset)
        {
            return Bits & BitHelper.GetMask<T>(length, offset);
        }

        public T Push(T value, int length = 1)
        {
            return Bits = Bits << length | value;
        }

        public T Pop(int length = 1)
        {
            return Peek(length).Configure(Bits = Bits >>> length);
        }

        public static BitStack<T> operator <<(BitStack<T> value, int shiftAmount)
        {
            return new BitStack<T>(value.Bits << shiftAmount);
        }

        public static BitStack<T> operator >>(BitStack<T> value, int shiftAmount)
        {
            return new BitStack<T>(value.Bits >> shiftAmount);
        }

        public static BitStack<T> operator >>>(BitStack<T> value, int shiftAmount)
        {
            return new BitStack<T>(value.Bits >>> shiftAmount);
        }

        public static BitStack<T> operator &(BitStack<T> left, BitStack<T> right)
        {
            return new BitStack<T>(left.Bits & right.Bits);
        }

        public static BitStack<T> operator |(BitStack<T> left, BitStack<T> right)
        {
            return new BitStack<T>(left.Bits | right.Bits);
        }

        public static BitStack<T> operator ^(BitStack<T> left, BitStack<T> right)
        {
            return new BitStack<T>(left.Bits ^ right.Bits);
        }

        public static BitStack<T> operator ~(BitStack<T> value)
        {
            return new BitStack<T>(~value.Bits);
        }

        public static explicit operator BitStack<T>(T bits)
        {
            return new BitStack<T>(bits);
        }

        public static explicit operator T(BitStack<T> bitStack)
        {
            return bitStack.Bits;
        }
    }
}