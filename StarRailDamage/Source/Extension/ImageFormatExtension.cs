using System.Drawing.Imaging;

namespace StarRailDamage.Source.Extension
{
    public static class ImageFormatExtension
    {
        public static ImageFormat? Parse(string? value)
        {
            return value?.ToLower() switch
            {
                "png" => ImageFormat.Png,
                "jpg" or "jpeg" => ImageFormat.Jpeg,
                "bmp" => ImageFormat.Bmp,
                "ico" => ImageFormat.Icon,
                _ => default
            };
        }
    }
}