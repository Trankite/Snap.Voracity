using StarRailDamage.Source.Core.Setting;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Encode.QRCode;
using StarRailDamage.Source.Service.FileOpen;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace StarRailDamage.Source.Service.Terminal.Command.Support
{
    public class QRCodeProduce : ITerminalCommand
    {
        public string Name => "qrcode";

        public string Help => LocalString.ServiceTerminalSupportQRCodeProduceHelp;

        public string[] Parameters => [CONTENT, FILEPATH, PATHOPEN, USEVECTOR];

        private const string CONTENT = "i";

        private const string FILEPATH = "f";

        private const string PATHOPEN = "o";

        private const string USEVECTOR = "e";

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetParameter(CONTENT, out string? Content))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            string FileName = commandLine.GetParameter(FILEPATH);
            bool UseSvg = FileName.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) || commandLine.GetBoolParameter(USEVECTOR);
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = $"Qrcode.{(UseSvg ? "svg" : "png")}";
            }
            string FilePath = Path.Combine(LocalSetting.GetTempPath(), FileName);
            return Invoke(Content, FilePath, commandLine.GetBoolParameter(PATHOPEN), UseSvg);
        }

        public static ITerminalResponse Invoke(string content, string filePath, bool pathOpen = false, bool useSvg = false)
        {
            using FileOpenWrite Write = FileOpenWrite.Create(filePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode = QRCode.Create(Encoding.UTF8.GetBytes(content), ECCodeLevel.M);
            if (useSvg)
            {
                Qrcode.SaveToSvg(Write.Stream);
            }
            else
            {
                Qrcode.GetBitmap().SaveAndDisponse(Write.Stream, ImageFormat.Png);
            }
            return new TerminalResponse(true, FileHelper.PathOpen(Write.FullPath, pathOpen));
        }
    }
}