using System.Runtime.InteropServices;

namespace Common.Source.Model.DataStruct.Bitwise
{
    public static class TypeSize<T>
    {
        public static int Size { get; }

        public static int BitSize { get; }

        static TypeSize()
        {
            Size = Marshal.SizeOf<T>();
            BitSize = Size * 8;
        }
    }
}