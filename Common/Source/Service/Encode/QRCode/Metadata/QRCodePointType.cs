namespace Common.Source.Service.Encode.QRCode.Metadata
{
    public enum QRCodePointType : byte
    {
        Unused,

        Fixed,

        FixedPoint,

        Timing,

        Format,

        Version,

        Content,

        ContentPadding,

        ECCode,

        Padding
    }
}