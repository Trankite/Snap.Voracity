using System.Diagnostics;

namespace Common.Source.Service.Encode.QRCode.Metadata
{
    public static class QRCodePointExtension
    {
        [DebuggerStepThrough]
        public static void SetBit(this ref QRCodePoint point, bool hasBit)
        {
            point = new QRCodePoint((byte)(hasBit ? point.Bits | 1 : point.Bits & 0xfe));
        }

        [DebuggerStepThrough]
        public static void SetType(this ref QRCodePoint point, QRCodePointType type)
        {
            point = new QRCodePoint((byte)(((int)type << 1) | (point.Bits & 1)));
        }
    }
}