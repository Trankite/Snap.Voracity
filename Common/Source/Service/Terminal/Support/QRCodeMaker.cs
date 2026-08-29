using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Resource.Localization;
using Common.Source.Service.Encode.QRCode;
using Common.Source.Service.Terminal.Abstraction;
using System.Drawing;
using System.Text;

namespace Common.Source.Service.Terminal.Support
{
    public class QRCodeMaker : TerminalCommand
    {
        public override string Name => "qrcode";

        public override string FullName => LocalString.ServiceTerminalSupportQRCodeMakerFullName;

        public override string Help => LocalString.ServiceTerminalSupportQRCodeMakerHelp;

        public override string[] RequiredParameters => [CONTENT, FILEPATH];

        public override string[] OptionalParameters => [FILEFORMAT, FOREGROUND, BACKGROUND, PIXELSIZE, PADDING, VERSION, ENCODEMODE, ECCODELEVEL, MASKTYPE, PATHOPEN];

        private const string CONTENT = "text";

        private const string FILEPATH = "path";

        private const string FILEFORMAT = "format";

        private const string FOREGROUND = "fore";

        private const string BACKGROUND = "back";

        private const string PIXELSIZE = "pixel";

        private const string PADDING = "padding";

        private const string VERSION = "version";

        private const string ENCODEMODE = "mode";

        private const string ECCODELEVEL = "level";

        private const string MASKTYPE = "mask";

        private const string PATHOPEN = "open";

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            string Content = commandLine.GetParameter(CONTENT);
            string FilePath = commandLine.GetParameter(FILEPATH);
            if (!commandLine.TryGetParameter(FILEFORMAT, out string? FileFormat))
            {
                FileFormat = FileHelper.GetExtensionName(FilePath).NotEmpty("svg");
            }
            if (string.IsNullOrEmpty(Path.GetExtension(FilePath)))
            {
                FilePath = Path.ChangeExtension(FilePath, FileFormat);
            }
            QRCodeOptions Options = new();
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(FOREGROUND), out Color Foreground))
            {
                Options.Foreground = Foreground;
            }
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(BACKGROUND), out Color Background))
            {
                Options.Background = Background;
            }
            if (int.TryParse(commandLine.GetParameter(PIXELSIZE), out int Pixel) && Pixel > 0)
            {
                Options.Pixel = Pixel;
            }
            if (int.TryParse(commandLine.GetParameter(PADDING), out int Padding) && Padding >= 0)
            {
                Options.Padding = Padding;
            }
            Options.Version = commandLine.GetIntParameter(VERSION);
            if (EnumExtension.TryParse(commandLine.GetParameter(ENCODEMODE), out EncodeMode EncodeMode))
            {
                Options.EncodeMode = EncodeMode;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(ECCODELEVEL), out ECCodeLevel ECCodeLevel))
            {
                Options.ECCodeLevel = ECCodeLevel;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(MASKTYPE), out MaskType MaskType))
            {
                Options.MaskType = MaskType;
            }
            bool PathOpne = commandLine.GetBoolParameter(PATHOPEN);
            return Invoke(Content, FilePath, Options, PathOpne, FileFormat);
        }

        public static ITerminalResponse Invoke(string content, string filePath, QRCodeOptions options, bool pathOpen = false, string? format = default)
        {
            using FileOpenWrite Write = FileOpenWrite.Create(filePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode;
            byte[] UTF8Bytes = Encoding.UTF8.GetBytes(content);
            if (format.EqualsIgnoreCase("csv"))
            {
                (Qrcode = QRCode.Create(UTF8Bytes, options)).SaveToCsv(Write.Stream);
            }
            else if (format.EqualsIgnoreCase("svg"))
            {
                (Qrcode = QRCode.Create(UTF8Bytes, options)).SaveToSvg(Write.Stream, options);
            }
            else
            {
                return new TerminalResponse(false, LocalString.ServiceTerminalSupportQRCodeMakerUnSupportedFormat);
            }
            FileHelper.PathOpen(Write.FullPath, pathOpen);
            object[] FormatInfo = [Qrcode.EncodeMode, Qrcode.Version, Qrcode.ECCodeLevel, Qrcode.MaskType.ToInt()];
            return new TerminalResponse(true, LocalString.ServiceTerminalSupportQRCodeMakerDetails.SafeFormat(FormatInfo));
        }
    }
}