using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.Extension
{
    public static class BitmapExtension
    {
        public static void SaveAndDisponse(this Bitmap bitmap, Stream stream, ImageFormat? format = default)
        {
            try { bitmap.Save(stream, format ?? ImageFormat.Png); } catch { } finally { bitmap.Dispose(); }
        }

        public static BitmapImage GetBitmapImage(this Bitmap bitmap)
        {
            using MemoryStream Stream = new();
            bitmap.Save(Stream, ImageFormat.Png);
            return BitmapImageExtension.GetBitmapImage(Stream);
        }
    }
}