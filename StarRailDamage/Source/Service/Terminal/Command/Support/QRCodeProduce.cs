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

        public string[] Parameters => [CONTENT, FILEPATH, FILEFORMAT, FOREGROUND, BACKGROUND, PIXELSIZE, PADDING, PATHOPEN];

        private const string CONTENT = "text";

        private const string FILEPATH = "path";

        private const string FILEFORMAT = "format";

        private const string FOREGROUND = "fore";

        private const string BACKGROUND = "back";

        private const string PIXELSIZE = "pixel";

        private const string PADDING = "padding";

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
            if (!ColorExtension.TryFromHtml(commandLine.GetParameter(FOREGROUND), out Color Foreground))
            {
                Foreground = Color.Black;
            }
            if (!ColorExtension.TryFromHtml(commandLine.GetParameter(BACKGROUND), out Color Background))
            {
                Background = Color.White;
            }
            int PixelSize = commandLine.GetIntParameter(PIXELSIZE, 5);
            if (PixelSize <= 0)
            {
                return TerminalManage.GetUnlawfulParameterResponse();
            }
            int Padding = commandLine.GetIntParameter(PIXELSIZE, 20);
            bool PathOpne = commandLine.GetBoolParameter(PATHOPEN);
            return Invoke(Content, FilePath, Foreground, Background, PixelSize, Padding, PathOpne, FileFormat);
        }

        public static ITerminalResponse Invoke(string content, string filePath, Color foreground, Color background, int pixelSize, int padding, bool pathOpen = false, string? format = default)
        {
            using FileOpenWrite Write = FileOpenWrite.Create(filePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode = QRCode.Create(Encoding.UTF8.GetBytes(content), ECCodeLevel.M);
            if (string.Equals(format, "svg", StringComparison.OrdinalIgnoreCase))
            {
                Qrcode.SaveToSvg(Write.Stream, foreground, background, pixelSize, padding);
            }
            else
            {
                Bitmap Bitmap = Qrcode.GetBitmap(foreground, background, pixelSize, padding);
                Bitmap.SaveAndDisponse(Write.Stream, ImageFormatExtension.Parse(format, ImageFormat.Png));
            }
            return new TerminalResponse(true, FileHelper.PathOpen(Write.FullPath, pathOpen));
        }
    }
}