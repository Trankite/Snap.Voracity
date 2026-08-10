using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;

namespace StarRailDamage.Source.Extension
{
    public static class ImageFormatExtension
    {
        public static bool TryParse(string? value, [NotNullWhen(true)] out ImageFormat? imageFormat)
        {
            imageFormat = value?.ToLower() switch
            {
                "png" => ImageFormat.Png,
                "jpg" or "jpeg" => ImageFormat.Jpeg,
                "bmp" => ImageFormat.Bmp,
                "ico" => ImageFormat.Icon,
                _ => default
            };
            return imageFormat.IsNotNull();
        }
    }
}