using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Encode.QRCode;
using StarRailDamage.Source.Service.FileOpen;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class QRCodeProduce : ITerminalCommand
    {
        public string Name => "qrcode";

        public string FullName => LocalString.ServiceTerminalSupportQRCodeProduceFullName;

        public string Help => LocalString.ServiceTerminalSupportQRCodeProduceHelp;

        public string[] Parameters => [CONTENT, FILEPATH, FILEFORMAT, FOREGROUND, BACKGROUND, PIXELSIZE, PADDING, VERSION, ENCODEMODE, ECCODELEVEL, MASKTYPE, PATHOPEN];

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

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            string Content = commandLine.GetParameter(CONTENT);
            if (string.IsNullOrEmpty(Content))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            string FileName = commandLine.GetParameter(FILEPATH);
            string FileFormat = commandLine.GetParameter(FILEFORMAT);
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = $"Qrcode.{FileFormat.NotEmpty("png")}";
            }
            string FilePath = Path.Combine(LocalSetting.GetTempPath(), FileName);
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
            if (int.TryParse(commandLine.GetParameter(PADDING), out int Padding))
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
            QRCode Qrcode = QRCode.Create(Encoding.UTF8.GetBytes(content), options);
            if (string.Equals(format, "csv", StringComparison.OrdinalIgnoreCase))
            {
                Qrcode.SaveToCsv(Write.Stream);
            }
            else if (string.Equals(format, "svg", StringComparison.OrdinalIgnoreCase))
            {
                Qrcode.SaveToSvg(Write.Stream, options);
            }
            else
            {
                Qrcode.GetBitmap(options).SaveAndDisponse(Write.Stream, ImageFormatExtension.Parse(format, ImageFormat.Png));
            }
            return new TerminalResponse(true, FileHelper.PathOpen(Write.FullPath, pathOpen));
        }
    }
}