namespace Common.Source.Service.Encode.QRCode.Metadata
{
    public readonly struct QRCodePoint
    {
        public readonly byte Bits { get; }

        public readonly bool HasBit => (Bits & 1) != 0;

        public readonly QRCodePointType Type => (QRCodePointType)(Bits >> 1);

        public QRCodePoint(byte bits)
        {
            Bits = bits;
        }

        public static QRCodePoint Create(bool hasBit, QRCodePointType type)
        {
            return new QRCodePoint((byte)(((int)type << 1) | (hasBit ? 1 : 0)));
        }
    }
}