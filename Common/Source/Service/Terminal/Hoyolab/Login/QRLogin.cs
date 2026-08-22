using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Encode.QRCode;
using Common.Source.Service.Mission;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Passport.QRLogin;
using Common.Source.Web.Hoyolab.Passport.QRLogin.Status;
using Common.Source.Web.Request;
using Common.Source.Web.Response;
using System.Text;

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
            if (string.IsNullOrEmpty(guid))
            {
                guid = HoyolabTokenManage.GetGuid();
            }
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginCreate);
            ITerminalResponse<QRLoginResponseWrapper> CreateQRLoginResponse = await CreateQRLogin(guid, cancellationToken);
            if (!CreateQRLoginResponse.TryGetAnalyzedBody(out QRLoginResponseWrapper? CreateQRLoginContent))
            {
                return CreateQRLoginResponse;
            }
            string Url = CreateQRLoginContent.Url;
            string Ticket = CreateQRLoginContent.Ticket;
            QRCode QRCode = QRCode.Create(Encoding.UTF8.GetBytes(Url));
            using CancellationTokenSource CancellationTokenSource = new(TimeSpan.FromMinutes(5));
            using CancellationTokenSource LinkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token, cancellationToken);
            void ShowQRCodeDialog()
            {
                LinkedCancellationTokenSource.Cancel();
                throw new NotImplementedException();
            }
            STAThread.Start(ShowQRCodeDialog);
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginShowQRCode);
            ITerminalResponse<QRLoginStatusResponseWrapper> QRLoginStatusResponse = await CheckStatus(guid, Ticket, LinkedCancellationTokenSource.Token);
            LinkedCancellationTokenSource.Cancel();
            if (!QRLoginStatusResponse.TryGetAnalyzedBody(out QRLoginStatusResponseWrapper? QRLoginStatusContent))
            {
                return QRLoginStatusResponse;
            }
            HoyolabToken HoyolabToken = new(guid);
            QRLoginStatusResponseUserInfo UserInfo = QRLoginStatusContent.UserInfo;
            HoyolabToken.Aid = UserInfo.Aid;
            HoyolabToken.Mid = UserInfo.Mid;
            foreach (QRLoginStatusResponseToken TokenSource in QRLoginStatusContent.Tokens)
            {
                HoyolabTokenType TokenType = (HoyolabTokenType)TokenSource.TokenType;
                linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginGetToken.SafeFormat(TokenType));
                HoyolabToken.SetToken(TokenType, TokenSource.Token);
            }
            return await UserLogin.AsyncInvoke(HoyolabToken, linkedStream, cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid, CancellationToken cancellationToken = default)
        {
            QRLoginRequestBuilderFactory Factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            FinalizedResponse<QRLoginResponse> Response = await Factory.Create().SendAsync<QRLoginResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out QRLoginResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, AnalyedBody.Url, AnalyedBody);
            }
            return new TerminalResponse<QRLoginResponseWrapper>(false, Response.ToString());
        }

        public static async ValueTask<ITerminalResponse<QRLoginStatusResponseWrapper>> CheckStatus(string guid, string ticket, CancellationToken cancellationToken = default)
        {
            QRLoginStatusRequestBuilderFactory Factory = new QRLoginStatusRequestBuilderFactory().SetGuid(guid).SetTicket(ticket);
            while (!cancellationToken.IsCancellationRequested)
            {
                FinalizedResponse<QRLoginStatusResponse> Response = await Factory.Create().SendAsync<QRLoginStatusResponse>(cancellationToken);
                if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out QRLoginStatusResponseWrapper? AnalyedBody))
                {
                    if (Response.Body.GetStatus() == QRLoginStatus.Confirmed)
                    {
                        return TerminalResponse.Create(true, AnalyedBody);
                    }
                }
                else
                {
                    return new TerminalResponse<QRLoginStatusResponseWrapper>(false, Response.ToString());
                }
                try { await Task.Delay(2000, cancellationToken); } catch { }
            }
            return new TerminalResponse<QRLoginStatusResponseWrapper>(false, LocalString.ServiceTerminalHoyolabLoginQRLoginExceptionCanceled);
        }
    }
}