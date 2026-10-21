using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.FileStore.Metadata;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Dispose.FileClean;
using Common.Source.Service.Mission;
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
using System.Diagnostics;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class QRLogin : AsyncTerminalCommand
    {
        public override string Name => "qrlogin";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginQRLoginFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabLoginQRLoginHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_Guid];

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
            ITerminalResponse<QRLoginResponseWrapper> CreateQRLoginResponse = await CreateQRLogin(guid, cancellationToken).ConfigureAwait(false);
            if (!CreateQRLoginResponse.TryGetContent(out QRLoginResponseWrapper? CreateQRLoginContent))
            {
                return CreateQRLoginResponse;
            }
            string Url = CreateQRLoginContent.Url;
            string Ticket = CreateQRLoginContent.Ticket;
            FileFormat QRCodeFormat = FileFormat.Svg;
            string QRCodeName = QRCodeFormat.ChangeExtension(nameof(QRLogin));
            string QRCodePath = Path.Combine(LocalSetting.GetTempPath(), QRCodeName);
            ITerminalResponse CreateQRCodeResponse = QRCodeMaker.Invoke(Url, QRCodePath);
            if (!CreateQRCodeResponse.Success)
            {
                return CreateQRCodeResponse;
            }
            using FileCleaner Cleaner = new(QRCodePath, true);
            using Process QRCodeProcess = ProcessHelper.Start(QRCodePath, true);
            using CancellationTokenSource TimeOutSource = new(TimeSpan.FromMinutes(5));
            using CancellationTokenSource CheckStatusSource = CancellationTokenSource.CreateLinkedTokenSource(TimeOutSource.Token, cancellationToken);
            textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginShowQRCode);
            while (await textStream.EnquireAsync(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckLogin, CheckStatusSource.Token))
            {
                ITerminalResponse<QRLoginStatusResponseWrapper> QRLoginStatusResponse = await CheckStatus(guid, Ticket, CheckStatusSource.Token).ConfigureAwait(false);
                if (!QRLoginStatusResponse.TryGetContent(out QRLoginStatusResponseWrapper? QRLoginStatusContent))
                {
                    return QRLoginStatusResponse;
                }
                if (EnumExtension.Parse<QRLoginStatus>(QRLoginStatusContent.Status) == QRLoginStatus.Confirmed)
                {
                    HoyolabToken HoyolabToken = new(guid);
                    QRLoginStatusResponseUserInfo UserInfo = QRLoginStatusContent.UserInfo;
                    HoyolabToken.Aid = UserInfo.Aid;
                    HoyolabToken.Mid = UserInfo.Mid;
                    foreach (QRLoginStatusResponseToken TokenSource in QRLoginStatusContent.Tokens)
                    {
                        HoyolabTokenType TokenType = (HoyolabTokenType)TokenSource.TokenType;
                        textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginGetToken.SafeFormat(TokenType));
                        HoyolabToken.SetToken(TokenType, TokenSource.Token);
                    }
                    return await UserLogin.AsyncInvoke(HoyolabToken, textStream, cancellationToken).ConfigureAwait(false);
                }
                textStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckStatus.SafeFormat(QRLoginStatusContent.Status));
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabLoginQRLoginExceptionCanceled);
        }

        private static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid, CancellationToken cancellationToken = default)
        {
            QRLoginRequestBuilderFactory Factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            HttpResponse<QRLoginResponse> Response = await Factory.Create().SendAsync<QRLoginResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetContent(out QRLoginResponseWrapper? QRLoginInfo))
            {
                return TerminalResponse.Create(true, QRLoginInfo);
            }
            return new TerminalResponse<QRLoginResponseWrapper>(false, Response.ToString());
        }

        private static async ValueTask<ITerminalResponse<QRLoginStatusResponseWrapper>> CheckStatus(string guid, string ticket, CancellationToken cancellationToken = default)
        {
            QRLoginStatusRequestBuilderFactory Factory = new QRLoginStatusRequestBuilderFactory().SetGuid(guid).SetTicket(ticket);
            HttpResponse<QRLoginStatusResponse> Response = await Factory.Create().SendAsync<QRLoginStatusResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetContent(out QRLoginStatusResponseWrapper? QRLoginStatus))
            {
                return TerminalResponse.Create(true, QRLoginStatus);
            }
            return new TerminalResponse<QRLoginStatusResponseWrapper>(false, Response.ToString());
        }
    }
}