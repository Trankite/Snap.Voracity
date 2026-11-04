using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace UIDesign.Source.Extension
{
    public static class BitmapExtension
    {
        [DebuggerStepThrough]
        public static void SaveAndDisponse(this Bitmap bitmap, Stream stream, ImageFormat? format = default)
        {
            try { bitmap.Save(stream, format ?? ImageFormat.Png); } catch { } finally { bitmap.Dispose(); }
        }

        [DebuggerStepThrough]
        public static BitmapImage GetBitmapImage(this Bitmap bitmap)
        {
            using MemoryStream stream = new();
            bitmap.Save(stream, ImageFormat.Png);
            return BitmapImageExtension.GetBitmapImage(stream);
        }
    }
}