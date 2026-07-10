using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Encode.QRCode;
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

        public override string[] Parameters => [GUID];

        private const string GUID = "guid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            return await AsyncInvoke(commandLine.GetParameter(GUID));
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string? guid = null)
        {
            if (string.IsNullOrEmpty(guid))
            {
                guid = HoyolabTokenManage.GetGuid();
            }
            ITerminalResponse<QRLoginResponseWrapper> CreateQRLoginResponse = await CreateQRLogin(guid);
            if (!CreateQRLoginResponse.Success || CreateQRLoginResponse.Content.IsNull())
            {
                return CreateQRLoginResponse;
            }
            using Bitmap Bitmap = QRCode.Create(Encoding.UTF8.GetBytes(CreateQRLoginResponse.Content.Url)).GetBitmap(new QRCodeOptions());
            using CancellationTokenSource CancellationTokenSource = new();
            Thread Thread = new(() =>
            {
                Window Window = new()
                {
                    Title = nameof(QRLogin),
                    Width = Bitmap.Width + 20,
                    Height = Bitmap.Height + 20,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = Bitmap.GetBitmapImage().GetImage()
                };
                Window.Closed += (sender, e) => CancellationTokenSource.Cancel();
                using (CancellationTokenSource.Token.Register(() => Window.Dispatcher.Invoke(Window.Close)))
                {
                    Window.ShowDialog();
                }
            });
            Thread.SetBackground().STAStart();
            ITerminalResponse<QRLoginStatusResponseWrapper> CheckStatusResponse = await CheckStatus(guid, CreateQRLoginResponse.Content.Ticket, CancellationTokenSource.Token);
            CancellationTokenSource.Cancel();
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
                HoyolabToken.SetToken((HoyolabTokenType)TokenSource.TokenType, TokenSource.Token);
            }
            return await UserLogin.AsyncInvoke(HoyolabToken);
        }

        public static async ValueTask<ITerminalResponse<QRLoginResponseWrapper>> CreateQRLogin(string guid)
        {
            QRLoginRequestBuilderFactory Factory = new QRLoginRequestBuilderFactory().SetGuid(guid);
            FinalizedResponse<QRLoginResponse> Response = await Factory.Create().SendAsync<QRLoginResponse>(Program.HttpClient);
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
                await Task.Delay(1500, CancellationToken.None);
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
            }
            return new TerminalResponse<QRLoginStatusResponseWrapper>(false);
        }
    }
}