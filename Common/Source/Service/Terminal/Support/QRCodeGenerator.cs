using Common.Source.Extension;
using Common.Source.Factory.FileStore;
using Common.Source.Resource.Localization;
using Common.Source.Service.Encode.QRCode;
using Common.Source.Service.Encode.QRCode.Metadata;
using Common.Source.Service.Streams.Metadata;
using Common.Source.Service.Streams.Open;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using System.Collections.Immutable;
using System.Drawing;

namespace Common.Source.Service.Terminal.Support
{
    public class QRCodeGenerator : TerminalCommand
    {
        public override string Name => "qrcode";

        public override string FullName => LocalString.ServiceTerminalSupportQRCodeGeneratorFullName;

        public override string Help => LocalString.ServiceTerminalSupportQRCodeGeneratorHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_Content, Param_FilePath];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_FileFormat, Param_Foreground, Param_Background, Param_PixelSize, Param_Padding, Param_Version, Param_EncodeMode, Param_ECCodeLevel, Param_MaskType, Param_PathOpen];

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

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            string content = commandLine.GetParameter(Param_Content);
            string filePath = commandLine.GetParameter(Param_FilePath);
            if (!commandLine.TryGetParameter(Param_FileFormat, out string? format))
            {
                format = FileHelper.GetExtensionName(filePath);
            }
            FileFormat fileFormat = FileFormatExtension.Parse(format, FileFormat.Svg);
            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
            {
                filePath = Path.ChangeExtension(filePath, fileFormat.GetDescription());
            }
            QRCodeOptions options = new();
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(Param_Foreground), out Color foreground))
            {
                options.Foreground = foreground;
            }
            if (ColorExtension.TryFromHtml(commandLine.GetParameter(Param_Background), out Color background))
            {
                options.Background = background;
            }
            if (int.TryParse(commandLine.GetParameter(Param_PixelSize), out int pixel) && pixel > 0)
            {
                options.Pixel = pixel;
            }
            if (int.TryParse(commandLine.GetParameter(Param_Padding), out int padding) && padding >= 0)
            {
                options.Padding = padding;
            }
            options.Version = commandLine.GetIntParameter(Param_Version);
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_EncodeMode), out EncodeMode encodeMode))
            {
                options.EncodeMode = encodeMode;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_ECCodeLevel), out ECCodeLevel eccodeLevel))
            {
                options.ECCodeLevel = eccodeLevel;
            }
            if (EnumExtension.TryParse(commandLine.GetParameter(Param_MaskType), out MaskType maskType))
            {
                options.MaskType = maskType;
            }
            bool pathOpne = commandLine.GetBoolParameter(Param_PathOpen);
            return Invoke(content, filePath, options, pathOpne, fileFormat);
        }

        public static ITerminalResponse Invoke(string content, string filePath, QRCodeOptions? options = default, bool pathOpen = false, FileFormat format = FileFormat.Svg)
        {
            options ??= new QRCodeOptions();
            using FileOpenWrite fileWrite = FileOpenWrite.Create(filePath, true);
            if (!fileWrite.IsReady)
            {
                return new TerminalResponse(false, fileWrite.ToString());
            }
            QRCode qrcode = QRCode.Create(content, options);
            QRCodeStore store = QRCodeStore.Create(qrcode, options);
            if (!store.IsSupported(format))
            {
                return new TerminalResponse(false, format.GetUnSupportedMessage());
            }
            store.SaveToFormat(fileWrite.Stream, format);
            FileHelper.PathOpen(fileWrite.FileInfo.FullName, pathOpen);
            object[] formatInfo = [qrcode.EncodeMode, qrcode.Version, qrcode.ECCodeLevel, qrcode.MaskType];
            return new TerminalResponse(true, LocalString.ServiceTerminalSupportQRCodeGeneratorDetails.SafeFormat(formatInfo));
        }
    }
}