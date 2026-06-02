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
        public static Bitmap GetBitmap(this QRCode qrcode, QRCodeOptions options)
        {
            int Pixel = options.Pixel;
            int Padding = options.Padding;
            int Size = Padding * 2 + qrcode.Size * Pixel;
            Bitmap Bitmap = new(Size, Size, PixelFormat.Format32bppArgb);
            try
            {
                using Brush Brush = new SolidBrush(options.Foreground);
                using Graphics Graphic = Graphics.FromImage(Bitmap);
                Graphic.CompositingMode = CompositingMode.SourceCopy;
                Graphic.Clear(options.Background);
                for (int x = 0; x < qrcode.Size; x++)
                {
                    for (int y = 0; y < qrcode.Size; y++)
                    {
                        if (qrcode[x, y].HasBit)
                        {
                            Graphic.FillRectangle(Brush, new Rectangle(Padding + x * Pixel, Padding + y * Pixel, Pixel, Pixel));
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

        public static void SaveToSvg(this QRCode qrcode, Stream stream, QRCodeOptions options)
        {
            const string LINKNAME = "i";
            const string WIDTHNAME = "width";
            const string HEIGHTNAME = "height";
            const string SVGNAMESPACE = "http://www.w3.org/2000/svg";
            const string XLINKNAMESPACE = "http://www.w3.org/1999/xlink";
            int Pixel = options.Pixel;
            int Padding = options.Padding;
            int Size = Padding * 2 + qrcode.Size * Pixel;
            using XmlWriter Writer = XmlWriter.Create(stream);
            Writer.WriteStartElement("svg", SVGNAMESPACE);
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("viewBox", $"0 0 {Size} {Size}");
            Writer.WriteAttributeString("xlink", XNamespace.Xmlns.NamespaceName, XLINKNAMESPACE);
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(options.Background));
            Writer.WriteEndElement();
            Writer.WriteStartElement("defs");
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString("id", LINKNAME);
            Writer.WriteAttributeString(WIDTHNAME, $"{Pixel}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Pixel}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(options.Foreground));
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
                        Writer.WriteAttributeString("x", $"{Padding + x * Pixel}");
                        Writer.WriteAttributeString("y", $"{Padding + y * Pixel}");
                        Writer.WriteAttributeString("href", XLINKNAMESPACE, $"#{LINKNAME}");
                        Writer.WriteEndElement();
                    }
                }
            }
            Writer.WriteEndElement();
            Writer.WriteEndElement();
        }

        public static void SaveToCsv(this QRCode qrcode, Stream stream)
        {
            using StreamWriter Writer = new(stream);
            ReadOnlySpan<char> BlackSpan = [',', '1'];
            ReadOnlySpan<char> WhiteSpan = [',', '0'];
            for (int x = 0; x < qrcode.Size; x++)
            {
                Writer.Write(qrcode[x, 0].HasBit ? '1' : '0');
                for (int y = 1; y < qrcode.Size; y++)
                {
                    Writer.Write(qrcode[x, y].HasBit ? BlackSpan : WhiteSpan);
                }
                Writer.WriteLine();
            }
        }
    }
}