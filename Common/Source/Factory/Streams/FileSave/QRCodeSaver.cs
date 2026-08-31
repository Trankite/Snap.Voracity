using Common.Source.Factory.Streams.FileSave.Abstract;
using Common.Source.Factory.Streams.FileSave.Metadata;
using Common.Source.Service.Encode.QRCode;
using System.Collections.Frozen;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;

namespace Common.Source.Factory.Streams.FileSave
{
    public class QRCodeSaver : FileFormatSaver<QRCodeSaver>
    {
        protected override QRCodeSaver Sender => this;

        private static readonly FrozenDictionary<FileFormat, Action<QRCodeSaver, Stream>> _FileSaveActionTable;

        protected override FrozenDictionary<FileFormat, Action<QRCodeSaver, Stream>> FileSaveActionTable => _FileSaveActionTable;

        private readonly QRCode QRCode;

        private readonly QRCodeOptions Options;

        public QRCodeSaver(QRCode qrcode, QRCodeOptions options)
        {
            QRCode = qrcode;
            Options = options;
        }

        public static QRCodeSaver Create(QRCode qrcode, QRCodeOptions? options = default)
        {
            return new QRCodeSaver(qrcode, options ?? new QRCodeOptions());
        }

        private static void SaveToSvg(QRCodeSaver saver, Stream stream)
        {
            const string LINKNAME = "i";
            const string WIDTHNAME = "width";
            const string HEIGHTNAME = "height";
            const string SVGNAMESPACE = "http://www.w3.org/2000/svg";
            const string XLINKNAMESPACE = "http://www.w3.org/1999/xlink";
            QRCode QRCode = saver.QRCode;
            QRCodeOptions Options = saver.Options;
            int Pixel = Options.Pixel;
            int Padding = Options.Padding;
            int Size = Padding * 2 + QRCode.Size * Pixel;
            using XmlWriter Writer = XmlWriter.Create(stream);
            Writer.WriteStartElement("svg", SVGNAMESPACE);
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("viewBox", $"0 0 {Size} {Size}");
            Writer.WriteAttributeString("xlink", XNamespace.Xmlns.NamespaceName, XLINKNAMESPACE);
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString(WIDTHNAME, $"{Size}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Size}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(Options.Background));
            Writer.WriteEndElement();
            Writer.WriteStartElement("defs");
            Writer.WriteStartElement("rect");
            Writer.WriteAttributeString("id", LINKNAME);
            Writer.WriteAttributeString(WIDTHNAME, $"{Pixel}");
            Writer.WriteAttributeString(HEIGHTNAME, $"{Pixel}");
            Writer.WriteAttributeString("fill", ColorTranslator.ToHtml(Options.Foreground));
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteStartElement("g");
            for (int x = 0; x < QRCode.Size; x++)
            {
                for (int y = 0; y < QRCode.Size; y++)
                {
                    if (QRCode[x, y].HasBit)
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

        public static void SaveToCsv(QRCodeSaver saver, Stream stream)
        {
            using StreamWriter Writer = new(stream, leaveOpen: true);
            ReadOnlySpan<char> BlackSpan = [',', '1'];
            ReadOnlySpan<char> WhiteSpan = [',', '0'];
            QRCode QRCode = saver.QRCode;
            for (int x = 0; x < QRCode.Size; x++)
            {
                Writer.Write(QRCode[x, 0].HasBit ? '1' : '0');
                for (int y = 1; y < QRCode.Size; y++)
                {
                    Writer.Write(QRCode[x, y].HasBit ? BlackSpan : WhiteSpan);
                }
                Writer.WriteLine();
            }
        }

        static QRCodeSaver()
        {
            _FileSaveActionTable = FrozenDictionary.Create(
                KeyValuePair.Create(FileFormat.Svg, SaveToSvg),
                KeyValuePair.Create(FileFormat.Csv, SaveToCsv)
            );
        }
    }
}