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
    public class QRCodeProduceCommand : ITerminalCommand
    {
        public string Name => "qrcode";

        public string Help => MarkedText.CommandHelpQRCodeProduce;

        public string[] Parameters => [CONTENT, FILEPATH, PATHOPEN];

        private const string CONTENT = "text";

        private const string FILEPATH = "path";

        private const string PATHOPEN = "open";

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetParameter(CONTENT, out string? Content))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            string FilePath = Path.Combine(LocalSetting.GetTempPath(), commandLine.GetParameter(FILEPATH).NotEmpty("QRCode.png"));
            return Invoke(Content, FilePath, commandLine.GetBoolParameter(PATHOPEN));
        }

        public static ITerminalResponse Invoke(string content, string filePath, bool pathOpen = false)
        {
            using FileOpenWrite Write = FileOpenWrite.Create(filePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode = QRCode.Create(Encoding.UTF8.GetBytes(content), ECCodeLevel.M);
            using (Bitmap Bitmap = Qrcode.GetBitmap())
            {
                Bitmap.Save(Write.Stream, ImageFormat.Png);
            }
            return new TerminalResponse(true, FileHelper.PathOpen(Write.FullPath, pathOpen));
        }
    }
}