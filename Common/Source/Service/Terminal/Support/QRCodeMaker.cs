using Common.Source.Extension;
using Common.Source.Factory.Streams.FileOpen;
using Common.Source.Factory.Streams.FileSave;
using Common.Source.Factory.Streams.FileSave.Abstract;
using Common.Source.Factory.Streams.FileSave.Metadata;
using Common.Source.Resource.Localization;
using Common.Source.Service.Encode.QRCode;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using System.Drawing;

namespace Common.Source.Service.Terminal.Support
{
    public class QRCodeMaker : TerminalCommand
    {
        public override string Name => "qrcode";

        public override string FullName => LocalString.ServiceTerminalSupportQRCodeMakerFullName;

        public override string Help => LocalString.ServiceTerminalSupportQRCodeMakerHelp;

        public override string[] RequiredParameters => [Param_Content, Param_FilePath];

        public override string[] OptionalParameters => [Param_FileFormat, Param_Foreground, Param_Background, Param_PixelSize, Param_Padding, Param_Version, Param_EncodeMode, Param_ECCodeLevel, Param_MaskType, Param_PathOpen];

        private const string Param_Content = TerminalParameters.Content;

        private const string Param_FilePath = TerminalParameters.FilePath;

        private const string Param_FileFormat = TerminalParameters.Format;

        private const string Param_Foreground = "fore";

        private const string Param_Background = "back";

        private const string Param_PixelSize = "pixel";

        private const string Param_Padding = "padding";

        private const string Param_Version = "version";

        private const string Param_EncodeMode = "mode";

        private const string Param_ECCodeLevel = "level";

        private const string Param_MaskType = "mask";

        private const string Param_PathOpen = TerminalParameters.PathOpen;

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            string Content = commandLine.GetParameter(Param_Content);
            string FilePath = commandLine.GetParameter(Param_FilePath);
            if (!commandLine.TryGetParameter(Param_FileFormat, out string? Format))
            {
                Format = FileHelper.GetExtensionName(FilePath);
            }
            FileFormat FileFormat = FileFormatExtension.Parse(Format, FileFormat.Svg);
            if (string.IsNullOrEmpty(Path.GetExtension(FilePath)))
            {
                FilePath = Path.ChangeExtension(FilePath, FileFormat.GetDescription());
            }
            QRCodeOptions Options = new();
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(Param_Foreground), out Color Foreground))
            {
                Options.Foreground = Foreground;
            }
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(Param_Background), out Color Background))
            {
                Options.Background = Background;
            }
            if (int.TryParse(commandLine.GetParameter(Param_PixelSize), out int Pixel) && Pixel > 0)
            {
                Options.Pixel = Pixel;
            }
            if (int.TryParse(commandLine.GetParameter(Param_Padding), out int Padding) && Padding >= 0)
            {
                Options.Padding = Padding;
            }
            Options.Version = commandLine.GetIntParameter(Param_Version);
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_EncodeMode), out EncodeMode EncodeMode))
            {
                Options.EncodeMode = EncodeMode;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_ECCodeLevel), out ECCodeLevel ECCodeLevel))
            {
                Options.ECCodeLevel = ECCodeLevel;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_MaskType), out MaskType MaskType))
            {
                Options.MaskType = MaskType;
            }
            bool PathOpne = commandLine.GetBoolParameter(Param_PathOpen);
            return Invoke(Content, FilePath, Options, PathOpne, FileFormat);
        }

        public static ITerminalResponse Invoke(string content, string filePath, QRCodeOptions? options = default, bool pathOpen = false, FileFormat format = FileFormat.Svg)
        {
            options ??= new QRCodeOptions();
            using FileOpenWrite Write = FileOpenWrite.Create(filePath);
            if (!Write.Success)
            {
                return new TerminalResponse(false, Write.ToString());
            }
            QRCode Qrcode = QRCode.Create(content, options);
            QRCodeSaver Saver = QRCodeSaver.Create(Qrcode, options);
            if (!Saver.TrySaveToFormat(Write.Stream, format))
            {
                return new TerminalResponse(false, format.UnSupported());
            }
            FileHelper.PathOpen(Write.FullPath, pathOpen);
            object[] FormatInfo = [Qrcode.EncodeMode, Qrcode.Version, Qrcode.ECCodeLevel, Qrcode.MaskType.ToInt()];
            return new TerminalResponse(true, LocalString.ServiceTerminalSupportQRCodeMakerDetails.SafeFormat(FormatInfo));
        }
    }
}