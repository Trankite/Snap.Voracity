using StarRailDamage.Source.Extension;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace StarRailDamage.Source.Service.Encode.QRCode
{
    public static class QRCodeExtension
    {
        private const int PIXEL = 5;

        private const int PADDING = 20;

        public static Bitmap GetBitmap(this QRCode qrcode, int pixelSize = PIXEL, int padding = PADDING)
        {
            return qrcode.GetBitmap(Color.Black, Color.White, pixelSize, padding);
        }

        public static Bitmap GetBitmap(this QRCode qrcode, Color black, Color white, int pixelSize = PIXEL, int padding = PADDING)
        {
            int Size = padding * 2 + qrcode.Size * pixelSize;
            Bitmap Bitmap = new(Size, Size, PixelFormat.Format32bppArgb);
            try
            {
                using Brush Brush = new SolidBrush(black);
                using Graphics Graphic = Graphics.FromImage(Bitmap);
                Graphic.CompositingMode = CompositingMode.SourceCopy;
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

        public static void SaveToSvg(this QRCode qrcode, Stream stream, int pixelSize = PIXEL, int padding = PADDING)
        {
            qrcode.SaveToSvg(stream, Color.Black, Color.White, pixelSize, padding);
        }

        public static void SaveToSvg(this QRCode qrcode, Stream stream, Color black, Color white, int pixelSize = PIXEL, int padding = PADDING)
        {
            const string LINKNAME = "i";
            const string WIDTHNAME = "width";
            const string HEIGHTNAME = "height";
            const string SVGNAMESPACE = "http://www.w3.org/2000/svg";
            const string XLINKNAMESPACE = "http://www.w3.org/1999/xlink";
            int Size = padding * 2 + qrcode.Size * pixelSize;
            using XmlWriter Writer = XmlWriter.Create(stream);
            Writer.WriteStartElement("svg", SVGNAMESPACE);
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("viewBox", $"0 0 {Size} {Size}");
            Writer.WriteAttributeString("xlink", XNamespace.Xmlns.NamespaceName, XLINKNAMESPACE);
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(white));
            Writer.WriteEndElement();
            Writer.WriteStartElement("defs");
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString("id", LINKNAME);
            Writer.WriteAttributeString(WIDTHNAME, $"{pixelSize}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{pixelSize}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(black));
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteStartElement("g");
            for (int x = 0; x < qrcode.Size; x++)
            {
                for (int y = 0; y < qrcode.Size; y++)
                {
                    if (qrcode[x, y].HasBit)
                    {
                        Writer.WriteStartElement("use");
                        Writer.WriteAttributeString("x", $"{padding + x * pixelSize}");
                        Writer.WriteAttributeString("y", $"{padding + y * pixelSize}");
                        Writer.WriteAttributeString("href", XLINKNAMESPACE, $"#{LINKNAME}");
                        Writer.WriteEndElement();
                    }
                }
            }
            Writer.WriteEndElement();
            Writer.WriteEndElement();
        }
    }
}