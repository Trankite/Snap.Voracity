using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StarRailDamage.Source.Extension
{
    public static class BitmapExtension
    {
        public static void SaveAndDisponse(this Bitmap bitmap, Stream stream, ImageFormat? format = default)
        {
            try { bitmap.Save(stream, format ?? ImageFormat.Png); } catch { } finally { bitmap.Dispose(); }
        }
    }
}