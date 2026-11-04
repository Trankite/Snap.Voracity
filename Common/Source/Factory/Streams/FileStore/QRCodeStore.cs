using Common.Source.Factory.Streams.FileStore.Abstract;
using Common.Source.Factory.Streams.FileStore.Metadata;
using Common.Source.Service.Encode.QRCode;
using System.Collections.Frozen;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;

namespace Common.Source.Factory.Streams.FileStore
{
    public class QRCodeStore : FileStore<QRCodeStore>
    {
        protected override QRCodeStore Sender => this;

        private static readonly FrozenDictionary<FileFormat, Action<QRCodeStore, Stream>> _FileSaveActionTable;

        protected override FrozenDictionary<FileFormat, Action<QRCodeStore, Stream>> FileSaveActionTable => _FileSaveActionTable;

        private readonly QRCode QRCode;

        private readonly QRCodeOptions Options;

        public QRCodeStore(QRCode qrcode, QRCodeOptions options)
        {
            QRCode = qrcode;
            Options = options;
        }

        public static QRCodeStore Create(QRCode qrcode, QRCodeOptions? options = default)
        {
            return new QRCodeStore(qrcode, options ?? new QRCodeOptions());
        }

        private static void SaveToSvg(QRCodeStore saver, Stream stream)
        {
            const string LINKNAME = "i";
            const string WIDTHNAME = "width";
            const string HEIGHTNAME = "height";
            const string SVGNAMESPACE = "http://www.w3.org/2000/svg";
            const string XLINKNAMESPACE = "http://www.w3.org/1999/xlink";
            QRCode qrcode = saver.QRCode;
            QRCodeOptions options = saver.Options;
            int pixel = options.Pixel;
            int padding = options.Padding;
            int size = padding * 2 + qrcode.Size * pixel;
            using XmlWriter writer = XmlWriter.Create(stream);
            writer.WriteStartElement("svg", SVGNAMESPACE);
            writer.WriteAttributeString(WIDTHNAME, $"{size}");
            writer.WriteAttributeString(HEIGHTNAME, $"{size}");
            writer.WriteAttributeString("viewBox", $"0 0 {size} {size}");
            writer.WriteAttributeString("xlink", XNamespace.Xmlns.NamespaceName, XLINKNAMESPACE);
            writer.WriteStartElement("rect");
            writer.WriteAttributeString(WIDTHNAME, $"{size}");
            writer.WriteAttributeString(HEIGHTNAME, $"{size}");
            writer.WriteAttributeString("fill", ColorTranslator.ToHtml(options.Background));
            writer.WriteEndElement();
            writer.WriteStartElement("defs");
            writer.WriteStartElement("rect");
            writer.WriteAttributeString("id", LINKNAME);
            writer.WriteAttributeString(WIDTHNAME, $"{pixel}");
            writer.WriteAttributeString(HEIGHTNAME, $"{pixel}");
            writer.WriteAttributeString("fill", ColorTranslator.ToHtml(options.Foreground));
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("g");
            for (int x = 0; x < qrcode.Size; x++)
            {
                for (int y = 0; y < qrcode.Size; y++)
                {
                    if (qrcode[x, y].HasBit)
                    {
                        writer.WriteStartElement("use");
                        writer.WriteAttributeString("x", $"{padding + x * pixel}");
                        writer.WriteAttributeString("y", $"{padding + y * pixel}");
                        writer.WriteAttributeString("href", XLINKNAMESPACE, $"#{LINKNAME}");
                        writer.WriteEndElement();
                    }
                }
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public static void SaveToCsv(QRCodeStore saver, Stream stream)
        {
            using StreamWriter writer = new(stream, leaveOpen: true);
            ReadOnlySpan<char> blackSpan = [',', '1'];
            ReadOnlySpan<char> whiteSpan = [',', '0'];
            QRCode qrcode = saver.QRCode;
            for (int x = 0; x < qrcode.Size; x++)
            {
                writer.Write(qrcode[x, 0].HasBit ? '1' : '0');
                for (int y = 1; y < qrcode.Size; y++)
                {
                    writer.Write(qrcode[x, y].HasBit ? blackSpan : whiteSpan);
                }
                writer.WriteLine();
            }
        }

        static QRCodeStore()
        {
            _FileSaveActionTable = FrozenDictionary.Create(
                KeyValuePair.Create(FileFormat.Svg, SaveToSvg),
                KeyValuePair.Create(FileFormat.Csv, SaveToCsv)
            );
        }
    }
}