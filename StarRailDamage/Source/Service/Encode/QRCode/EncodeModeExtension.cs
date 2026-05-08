using StarRailDamage.Source.Service.Encode.QRCode.Encoder;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public static class EncodeModeExtension
    {
        public static EncodeMode GetAutoMode(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                byte Current = data[i];
                if (Current < '0' || Current > '9')
                {
                    while (++i < data.Length)
                    {
                        if (!AlphaEncoder.IsValid(data[i]))
                        {
                            return EncodeMode.Byte;
                        }
                    }
                    return EncodeMode.Alphanumeric;
                }
            }
            return EncodeMode.Numeric;
        }

        public static QRCodeEncoder CreateEncoder(this EncodeMode mode)
        {
            return mode switch
            {
                EncodeMode.Numeric => new NumericEncoder(),
                EncodeMode.Alphanumeric => new AlphaEncoder(),
                EncodeMode.Byte => new ByteEncoder(),
                _ => throw new NotSupportedException($"Unkonw EncodeMode:{mode}")
            };
        }
    }
}