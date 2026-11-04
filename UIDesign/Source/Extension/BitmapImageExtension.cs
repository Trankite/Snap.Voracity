using Common.Source.Extension;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UIDesign.Source.Extension
{
    public static class BitmapImageExtension
    {
        public static readonly BitmapImage DefaultImage;

        [DebuggerStepThrough]
        public static BitmapImage SetFreeze(this BitmapImage value)
        {
            return value.Configure(value.Freeze);
        }

        [DebuggerStepThrough]
        public static BitmapImage GetBitmapImage(Stream stream)
        {
            stream.Position = 0;
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        [DebuggerStepThrough]
        public static Image GetImage(this BitmapImage image)
        {
            return new Image() { Source = image };
        }

        static BitmapImageExtension()
        {
            int dpi = 96;
            int width = 32;
            int height = 32;
            PngBitmapEncoder encoder = new();
            PixelFormat format = PixelFormats.Pbgra32;
            int pixel = (format.BitsPerPixel + 7) / 8;
            encoder.Frames.Add(BitmapFrame.Create(
                BitmapSource.Create(
                    width, height, dpi, dpi, format, null,
                    new int[pixel * width * height],
                    width * pixel)));
            using MemoryStream stream = new();
            encoder.Save(stream);
            DefaultImage = GetBitmapImage(stream);
        }
    }
}