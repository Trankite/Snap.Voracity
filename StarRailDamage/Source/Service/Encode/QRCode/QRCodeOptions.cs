using System.Drawing;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public class QRCodeOptions
    {
        public int Pixel { get; set; } = 5;

        public int Padding { get; set; } = 20;

        public Color Foreground { get; set; } = Color.Black;

        public Color Background { get; set; } = Color.White;

        public int Version { get; set; }

        public EncodeMode EncodeMode { get; set; } = EncodeMode.Optimal;

        public ECCodeLevel ECCodeLevel { get; set; } = ECCodeLevel.M;

        public MaskType MaskType { get; set; } = MaskType.Optimal;
    }
}