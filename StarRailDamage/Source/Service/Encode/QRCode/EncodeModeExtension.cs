using StarRailDamage.Source.Service.Encode.QRCode.Encoder;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public static class EncodeModeExtension
    {
        public static EncodeMode GetEncodeMode(ReadOnlySpan<byte> content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] < '0' || content[i] > '9')
                {
                    while (i < content.Length)
                    {
                        if (!AlphabetEncoder.IsValid(content[i++])) return EncodeMode.Byte;
                    }
                    return EncodeMode.Alphabet;
                }
            }
            return EncodeMode.Numeric;
        }

        public static QRCodeEncoder CreateEncoder(this EncodeMode mode)
        {
            return mode switch
            {
                EncodeMode.Numeric => new NumericEncoder(),
                EncodeMode.Alphabet => new AlphabetEncoder(),
                EncodeMode.Byte => new ByteEncoder(),
                _ => throw new NotSupportedException($"Unkonw EncodeMode:{mode}")
            };
        }
    }
}