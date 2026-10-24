namespace Common.Source.Service.Encode.QRCode
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