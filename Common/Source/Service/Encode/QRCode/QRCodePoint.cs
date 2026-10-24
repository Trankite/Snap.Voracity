namespace Common.Source.Service.Encode.QRCode
{
    public struct QRCodePoint
    {
        private byte Bits;

        public bool HasBit
        {
            readonly get => (Bits & 1) != 0;
            set => Bits = (byte)(value ? Bits | 1 : Bits & 0xfe);
        }

        public QRCodePointType Type
        {
            readonly get => (QRCodePointType)(Bits >> 1);
            set => Bits = (byte)(((int)value << 1) | (Bits & 1));
        }

        public void SetValue(bool hasBit, QRCodePointType type)
        {
            Bits = (byte)(((int)type << 1) | (hasBit ? 1 : 0));
        }

        public static implicit operator byte(QRCodePoint qrcodePoint)
        {
            return qrcodePoint.Bits;
        }
    }
}