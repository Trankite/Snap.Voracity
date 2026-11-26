using System.Diagnostics;
using System.Numerics;

namespace Common.Source.Service.Helper
{
    public static class BitHelper
    {
        [DebuggerStepThrough]
        public static T GetMask<T>(int length) where T : struct, IShiftOperators<T, int, T>, IBitwiseOperators<T, T, T>
        {
            return ~(~default(T) << length);
        }
    }
}