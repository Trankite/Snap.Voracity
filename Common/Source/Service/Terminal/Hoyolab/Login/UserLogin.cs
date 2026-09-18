using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Passport.Exchange;
using Common.Source.Web.Hoyolab.Takumi.DeviceFp;
using Common.Source.Web.Hoyolab.Takumi.GameRole;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class UserLogin : AsyncTerminalCommand
    {
        public override string Name => "login";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginUserLoginFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabLoginUserLoginHelp;

        public override string[] RequiredParameters => [Param_Mid, Param_Stoken];

        public override string[] OptionalParameters => [Param_Guid];

        private const string Param_Mid = "mid";

        private const string Param_Stoken = "stoken";

        private const string Param_Guid = "guid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            if (!commandLine.TryGetParameter(Param_Guid, out string? Guid))
            {
                Guid = HoyolabTokenManage.GetGuid();
            }
            HoyolabToken HoyolabToken = new(Guid) { Mid = commandLine.GetParameter(Param_Mid) };
            HoyolabToken.SetToken(HoyolabTokenType.SToken, commandLine.GetParameter(Param_Stoken));
            return await AsyncInvoke(HoyolabToken, linkedStream, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabToken hoyolabToken, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetDeviceFp);
            ITerminalResponse<DeviceFpResponseWrapper> DeviceFpResponse = await DeviceFp.AsyncInvoke(cancellationToken).ConfigureAwait(false);
            if (!DeviceFpResponse.TryGetAnalyzedBody(out DeviceFpResponseWrapper? DeviceFpContent))
            {
                return DeviceFpResponse;
            }
            hoyolabToken.Device = DeviceFpContent.DeviceFp;
            ExchangeRequestBuilderFactory ExchangeFactory = new ExchangeRequestBuilderFactory(hoyolabToken).SetOrigin(HoyolabTokenType.SToken);
            foreach (HoyolabTokenType TokenType in Enum.GetValues<HoyolabTokenType>())
            {
                if (!hoyolabToken.Tokens.ContainsKey(TokenType))
                {
                    ExchangeFactory.SetDestin(TokenType);
                    linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetToken.SafeFormat(TokenType));
                    FinalizedResponse<ExchangeResponse> ExchangeResponse = await ExchangeFactory.Create().SendAsync<ExchangeResponse>(cancellationToken).ConfigureAwait(false);
                    if (ExchangeResponse.Body.IsNull() || !ExchangeResponse.Body.TryGetAnalyzedBody(out ExchangeResponseToken? ExchangeAnalyedBody))
                    {
                        return new TerminalResponse(false, ExchangeResponse.ToString());
                    }
                    hoyolabToken.SetToken(TokenType, ExchangeAnalyedBody.Token);
                }
            }
            GameRoleRequestBuilderFactory GameRoleFactory = new(hoyolabToken);
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetUserRole);
            FinalizedResponse<GameRoleResponse> GameRoleResponse = await GameRoleFactory.Create().SendAsync<GameRoleResponse>(cancellationToken).ConfigureAwait(false);
            if (GameRoleResponse.Body.IsNull() || !GameRoleResponse.Body.TryGetAnalyzedBody(out HoyolabUserRole[]? GameRoleAnalyedBody))
            {
                return new TerminalResponse(false, GameRoleResponse.ToString());
            }
            hoyolabToken.UserRoles = GameRoleAnalyedBody;
            linkedStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginTryUpdate);
            try
            {
                await HoyolabTokenManage.Update(hoyolabToken, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception Exception)
            {
                return new TerminalResponse(false, Exception.Message);
            }
            return new TerminalResponse(true, LocalString.ServiceTerminalHoyolabLoginUserLoginSuccess);
        }
    }
}