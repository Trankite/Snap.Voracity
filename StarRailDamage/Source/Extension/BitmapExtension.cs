using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StarRailDamage.Source.Extension
{
    public static class BitmapExtension
    {
        public static void SaveAndDisponse(this Bitmap bitmap, Stream stream, ImageFormat format)
        {
            try { bitmap.Save(stream, format); } catch { } finally { bitmap.Dispose(); }
        }
    }
}