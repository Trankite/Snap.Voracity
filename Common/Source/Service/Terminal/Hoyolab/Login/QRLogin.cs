using Common.Source.Core.Setting;
using Common.Source.Extension;
using Common.Source.Factory.Streams.FileClean;
using Common.Source.Factory.Streams.FileSave.Metadata;
using Common.Source.Resource.Localization;
using Common.Source.Service.Mission;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Service.Terminal.Support;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Passport.QRLogin;
using Common.Source.Web.Hoyolab.Passport.QRLogin.Status;
using Common.Source.Web.Request;
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

        public override string[] OptionalParameters => [GUID];

        private const string GUID = "guid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(GUID), linkedStream, cancellationToken);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string? guid = default, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            if (linkedStream.IsNull())
            {
                return TerminalManage.GetMissingUserInteractionResponse();
            }
            if (string.IsNullOrEmpty(guid))
            {
                guid = HoyolabTokenManage.GetGuid();
            }
            linkedStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCreate);
            ITerminalResponse<QRLoginResponseWrapper> CreateQRLoginResponse = await CreateQRLogin(guid, cancellationToken);
            if (!CreateQRLoginResponse.TryGetAnalyzedBody(out QRLoginResponseWrapper? CreateQRLoginContent))
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
            linkedStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginShowQRCode);
            while (await linkedStream.EnquireAsync(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckLogin, CheckStatusSource.Token))
            {
                ITerminalResponse<QRLoginStatusResponseWrapper> QRLoginStatusResponse = await CheckStatus(guid, Ticket, CheckStatusSource.Token);
                if (!QRLoginStatusResponse.TryGetAnalyzedBody(out QRLoginStatusResponseWrapper? QRLoginStatusContent))
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
                        linkedStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginGetToken.SafeFormat(TokenType));
                        HoyolabToken.SetToken(TokenType, TokenSource.Token);
                    }
                    return await UserLogin.AsyncInvoke(HoyolabToken, linkedStream, cancellationToken);
                }
                linkedStream.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCheckStatus.SafeFormat(QRLoginStatusContent.Status));
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabLoginQRLoginExceptionCanceled);
        }

        private static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid, CancellationToken cancellationToken = default)
        {
            QRLoginRequestBuilderFactory Factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            FinalizedResponse<QRLoginResponse> Response = await Factory.Create().SendAsync<QRLoginResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out QRLoginResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, AnalyedBody);
            }
            return new TerminalResponse<QRLoginResponseWrapper>(false, Response.ToString());
        }

        private static async ValueTask<ITerminalResponse<QRLoginStatusResponseWrapper>> CheckStatus(string guid, string ticket, CancellationToken cancellationToken = default)
        {
            QRLoginStatusRequestBuilderFactory Factory = new QRLoginStatusRequestBuilderFactory().SetGuid(guid).SetTicket(ticket);
            FinalizedResponse<QRLoginStatusResponse> Response = await Factory.Create().SendAsync<QRLoginStatusResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out QRLoginStatusResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, AnalyedBody);
            }
            return new TerminalResponse<QRLoginStatusResponseWrapper>(false, Response.ToString());
        }
    }
}