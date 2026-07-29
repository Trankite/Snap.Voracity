using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Encode.QRCode;
using StarRailDamage.Source.Service.Mission;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Passport.QRLogin;
using StarRailDamage.Source.Web.Hoyolab.Passport.QRLogin.Status;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;
using System.Drawing;
using System.Text;
using System.Windows;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Login
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
            if (!CreateQRLoginResponse.Success || CreateQRLoginResponse.Content.IsNull())
            {
                return CreateQRLoginResponse;
            }
            string Url = CreateQRLoginResponse.Content.Url;
            string Ticket = CreateQRLoginResponse.Content.Ticket;
            using Bitmap Bitmap = QRCode.Create(Encoding.UTF8.GetBytes(Url)).GetBitmap(new QRCodeOptions());
            using CancellationTokenSource CancellationTokenSource = new(TimeSpan.FromMinutes(5));
            using CancellationTokenSource LinkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token, cancellationToken);
            STAThread.Start(() =>
            {
                Window Window = new()
                {
                    Title = nameof(QRLogin),
                    Width = Bitmap.Width + 20,
                    Height = Bitmap.Height + 20,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = Bitmap.GetBitmapImage().GetImage()
                };
                Window.Closed += (sender, e) => LinkedCancellationTokenSource.Cancel();
                using (LinkedCancellationTokenSource.Token.Register(() => Window.Dispatcher.Invoke(Window.Close)))
                {
                    Window.ShowDialog();
                }
            });
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginShowQRCode);
            ITerminalResponse<QRLoginStatusResponseWrapper> CheckStatusResponse = await CheckStatus(guid, Ticket, LinkedCancellationTokenSource.Token);
            LinkedCancellationTokenSource.Cancel();
            if (!CheckStatusResponse.Success || CheckStatusResponse.Content.IsNull())
            {
                return CheckStatusResponse;
            }
            HoyolabToken HoyolabToken = new(guid);
            QRLoginStatusResponseUserInfo UserInfo = CheckStatusResponse.Content.UserInfo;
            HoyolabToken.Aid = UserInfo.Aid;
            HoyolabToken.Mid = UserInfo.Mid;
            foreach (QRLoginStatusResponseToken TokenSource in CheckStatusResponse.Content.Tokens)
            {
                HoyolabTokenType TokenType = (HoyolabTokenType)TokenSource.TokenType;
                linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginQRLoginGetToken.Format(TokenType));
                HoyolabToken.SetToken(TokenType, TokenSource.Token);
            }
            return await UserLogin.AsyncInvoke(HoyolabToken, linkedStream, cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid, CancellationToken cancellationToken = default)
        {
            QRLoginRequestBuilderFactory Factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            FinalizedResponse<QRLoginResponse> Response = await Factory.Create().SendAsync<QRLoginResponse>(Program.HttpClient, cancellationToken);
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
                FinalizedResponse<QRLoginStatusResponse> Response = await Factory.Create().SendAsync<QRLoginStatusResponse>(Program.HttpClient, cancellationToken);
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