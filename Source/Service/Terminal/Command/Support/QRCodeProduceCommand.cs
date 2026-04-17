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

        public ITerminalResponse Invoke(params IList<string> parameter)
        {
            if (!parameter.TryGetFirst(out string? ContentText))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            string? FilePath = parameter.Index(1);
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = Path.Combine(LocalSetting.GetTempPath(), "QRCode.png");
            }
            if (!EnumExtension.TryParse(parameter.Index(2), out ECCodeLevel Level))
            {
                Level = ECCodeLevel.M;
            }
            if (!EnumExtension.TryParse(parameter.Index(3), out MaskType MaskType))
            {
                MaskType = MaskType.Mask000;
            }
            bool PathOpen = BoolExtension.Parse(parameter.Index(4));
            if (!ColorExtension.TryFromHtml(parameter.Index(5), out Color Black))
            {
                Black = Color.Black;
            }
            if (!ColorExtension.TryFromHtml(parameter.Index(6), out Color White))
            {
                White = Color.White;
            }
            using FileOpenWrite Write = FileOpenWrite.Create(FilePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode = QRCode.Create(Encoding.UTF8.GetBytes(ContentText), Level, MaskType);
            using (Bitmap Bitmap = Qrcode.GetBitmap(Black, White))
            {
                Bitmap.Save(Write.Stream, ImageFormat.Png);
            }
            return new TerminalResponse(true, PathOpen ? FileHelper.PathOpen(FilePath) : FilePath);
        }
    }
}