using StarRailDamage.Source.Extension;
using System.Drawing;
using System.Drawing.Imaging;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public static class QRCodeExtension
    {
        public static Bitmap GetBitmap(this QRCode qrcode, int pixelSize = 5, int padding = 20)
        {
            return qrcode.GetBitmap(Color.Black, Color.White, pixelSize, padding);
        }

        public static Bitmap GetBitmap(this QRCode qrcode, Color black, Color white, int pixelSize = 5, int padding = 20)
        {
            int Size = padding * 2 + qrcode.Size * pixelSize;
            Bitmap Bitmap = new(Size, Size, PixelFormat.Format24bppRgb);
            try
            {
                using Brush Brush = new SolidBrush(black);
                using Graphics Graphic = Graphics.FromImage(Bitmap);
                Graphic.Clear(white);
                for (int x = 0; x < qrcode.Size; x++)
                {
                    for (int y = 0; y < qrcode.Size; y++)
                    {
                        if (qrcode[x, y].HasBit)
                        {
                            Graphic.FillRectangle(Brush, new Rectangle(padding + x * pixelSize, padding + y * pixelSize, pixelSize, pixelSize));
                        }
                    }
                }
                return Bitmap;
            }
            catch (Exception Exception)
            {
                throw new Exception(Exception.Message, Exception).Configure(Bitmap.Dispose);
            }
        }
    }
}