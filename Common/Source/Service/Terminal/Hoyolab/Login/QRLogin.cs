using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Helper;
using Common.Source.Service.Helper.Dispose.FileClean;
using Common.Source.Service.Streams.Metadata;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Service.Terminal.Support;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Passport.QRLogin;
using Common.Source.Web.Hoyolab.Passport.QRLogin.Status;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class QRLogin : AsyncTerminalCommand
    {
        public override string Name => "qrlogin";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginQRLoginFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabLoginQRLoginHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Guid];

        private const string Param_Guid = "guid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Guid), textStream, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string? guid = default, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            if (textStream.IsNull())
            {
                return SupportTerminalResponse.MissingUserInteraction();
            }
            if (string.IsNullOrEmpty(guid))
            {
                guid = HoyolabTokenManage.GetGuid();
            }
            textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCreate);
            ITerminalResponse<QRLoginResponseWrapper> createQRLoginResponse = await CreateQRLogin(guid, cancellationToken).ConfigureAwait(false);
            if (!createQRLoginResponse.TryGetContent(out QRLoginResponseWrapper? createQRLoginContent))
            {
                return createQRLoginResponse;
            }
            string url = createQRLoginContent.Url;
            string ticket = createQRLoginContent.Ticket;
            FileFormat qrcodeFormat = FileFormat.Svg;
            string qrcodeName = qrcodeFormat.ChangeExtension(nameof(QRLogin));
            string qrcodePath = Path.Combine(LocalSetting.GetTempPath(), qrcodeName);
            ITerminalResponse createQRCodeResponse = QRCodeGenerator.Invoke(url, qrcodePath);
            if (!createQRCodeResponse.Success)
            {
                return createQRCodeResponse;
            }
            using FileCleaner cleaner = new(qrcodePath, true);
            using Process qrcodeProcess = ProcessHelper.Start(qrcodePath, true);
            using CancellationTokenSource checkStatusSource = cancellationToken.CreateSource();
            checkStatusSource.CancelAfter(TimeSpan.FromMinutes(5));
            textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginShowQRCode);
            while (await textStream.EnquireAsync(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckLogin, checkStatusSource.Token))
            {
                ITerminalResponse<QRLoginStatusResponseWrapper> qrloginStatusResponse = await CheckStatus(guid, ticket, checkStatusSource.Token).ConfigureAwait(false);
                if (!qrloginStatusResponse.TryGetContent(out QRLoginStatusResponseWrapper? qrloginStatusContent))
                {
                    return qrloginStatusResponse;
                }
                if (EnumExtension.Parse<QRLoginStatus>(qrloginStatusContent.Status) == QRLoginStatus.Confirmed)
                {
                    HoyolabToken hoyolabToken = new(guid);
                    QRLoginStatusResponseUserInfo userInfo = qrloginStatusContent.UserInfo;
                    hoyolabToken.Aid = userInfo.Aid;
                    hoyolabToken.Mid = userInfo.Mid;
                    foreach (QRLoginStatusResponseToken tokenSource in qrloginStatusContent.Tokens)
                    {
                        HoyolabTokenType tokenType = (HoyolabTokenType)tokenSource.TokenType;
                        textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginGetToken.SafeFormat(tokenType));
                        hoyolabToken.SetToken(tokenType, tokenSource.Token);
                    }
                    return await UserLogin.AsyncInvoke(hoyolabToken, textStream, cancellationToken).ConfigureAwait(false);
                }
                textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckStatus.SafeFormat(qrloginStatusContent.Status));
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabLoginQRLoginExceptionCanceled);
        }

        private static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid, CancellationToken cancellationToken = default)
        {
            QRLoginRequestBuilderFactory factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            HttpResponse<QRLoginResponse> response = await factory.Create().SendAsync<QRLoginResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TryGetContent(out QRLoginResponseWrapper? qrloginInfo))
            {
                return TerminalResponse.Create(true, qrloginInfo);
            }
            return new TerminalResponse<QRLoginResponseWrapper>(false, response.ToString());
        }

        private static async ValueTask<ITerminalResponse<QRLoginStatusResponseWrapper>> CheckStatus(string guid, string ticket, CancellationToken cancellationToken = default)
        {
            QRLoginStatusRequestBuilderFactory factory = new QRLoginStatusRequestBuilderFactory().SetGuid(guid).SetTicket(ticket);
            HttpResponse<QRLoginStatusResponse> response = await factory.Create().SendAsync<QRLoginStatusResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TryGetContent(out QRLoginStatusResponseWrapper? qrloginStatus))
            {
                return TerminalResponse.Create(true, qrloginStatus);
            }
            return new TerminalResponse<QRLoginStatusResponseWrapper>(false, response.ToString());
        }
    }
}