using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Passport.Exchange;
using StarRailDamage.Source.Web.Hoyolab.Takumi.DeviceFp;
using StarRailDamage.Source.Web.Hoyolab.Takumi.GameRole;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Login
{
    public class UserLogin : AsyncTerminalCommand
    {
        public override string Name => "login";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginUserLoginFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabLoginUserLoginHelp;

        public override string[] Parameters => [MID, STOKEN];

        private const string MID = "mid";

        private const string STOKEN = "stoken";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            HoyolabToken HoyolabToken = new() { Mid = commandLine.GetParameter(MID) };
            HoyolabToken.SetToken(HoyolabTokenType.SToken, commandLine.GetParameter(STOKEN));
            return await AsyncInvoke(HoyolabToken);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabToken hoyolabToken)
        {
            ITerminalResponse<DeviceFpResponseWrapper> DeviceFpResponse = await DeviceFp.AsyncInvoke();
            if (!DeviceFpResponse.Success || DeviceFpResponse.Content.IsNull())
            {
                return DeviceFpResponse;
            }
            hoyolabToken.Device = DeviceFpResponse.Content.DeviceFp;
            ExchangeRequestBuilderFactory ExchangeFactory = new ExchangeRequestBuilderFactory(hoyolabToken).SetOrigin(HoyolabTokenType.SToken);
            foreach (HoyolabTokenType TokenType in Enum.GetValues<HoyolabTokenType>())
            {
                if (!hoyolabToken.Tokens.ContainsKey(TokenType))
                {
                    ExchangeFactory.SetDestin(TokenType);
                    FinalizedResponse<ExchangeResponse> ExchangeResponse = await ExchangeFactory.Create().SendAsync<ExchangeResponse>(Program.HttpClient);
                    if (ExchangeResponse.Body.IsNull() || !ExchangeResponse.Body.TryGetAnalyzedBody(out ExchangeResponseToken? ExchangeAnalyedBody))
                    {
                        return new TerminalResponse(false, ExchangeResponse.ToString());
                    }
                    hoyolabToken.SetToken(TokenType, ExchangeAnalyedBody.Token);
                }
            }
            GameRoleRequestBuilderFactory GameRoleFactory = new(hoyolabToken);
            FinalizedResponse<GameRoleResponse> GameRoleResponse = await GameRoleFactory.Create().SendAsync<GameRoleResponse>(Program.HttpClient);
            if (GameRoleResponse.Body.IsNull() || !GameRoleResponse.Body.TryGetAnalyzedBody(out HoyolabUserRole[]? GameRoleAnalyedBody))
            {
                return new TerminalResponse(false, GameRoleResponse.ToString());
            }
            hoyolabToken.UserRoles = GameRoleAnalyedBody;
            try
            {
                await HoyolabTokenManage.Update(hoyolabToken);
            }
            catch (Exception Exception)
            {
                return new TerminalResponse(false, Exception.Message);
            }
            return new TerminalResponse(true, LocalString.ServiceTerminalHoyolabLoginUserLoginSuccess);
        }
    }
}