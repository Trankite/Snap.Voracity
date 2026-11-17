using Common.Source.Extension;
using Common.Source.Service.Encode.QRCode.Encoder;
using System.Diagnostics;

namespace Common.Source.Service.Encode.QRCode.Metadata
{
    public static class EncodeModeExtension
    {
        [DebuggerStepThrough]
        public static EncodeMode GetAutoEncodeMode(this EncodeMode mode, ReadOnlySpan<byte> content)
        {
            if (mode == EncodeMode.Byte)
            {
                return mode;
            }
            return GetEncodeMode(content).LessOrEquals(mode);
        }

        public static EncodeMode GetEncodeMode(ReadOnlySpan<byte> content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] < '0' || content[i] > '9')
                {
                    while (i < content.Length)
                    {
                        if (!AlphanumericEncoder.Exists(content[i++]))
                        {
                            return EncodeMode.Byte;
                        }
                    }
                    return EncodeMode.Alphanumeric;
                }
            }
            return EncodeMode.Numeric;
        }

        [DebuggerStepThrough]
        public static QRCodeEncoder CreateEncoder(this EncodeMode mode)
        {
            return mode switch
            {
                EncodeMode.Numeric => new NumericEncoder(),
                EncodeMode.Alphanumeric => new AlphanumericEncoder(),
                EncodeMode.Byte => new ByteEncoder(),
                _ => throw new NotSupportedException($"Unknown EncodeMode:{mode}")
            };
        }
    }
}