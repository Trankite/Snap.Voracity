using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Passport.Exchange;
using Common.Source.Web.Hoyolab.Takumi.DeviceFp;
using Common.Source.Web.Hoyolab.Takumi.GameRole;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class UserLogin : AsyncTerminalCommand
    {
        public override string Name => "login";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginUserLoginFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabLoginUserLoginHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_Mid, Param_Stoken];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Guid];

        private const string Param_Mid = "mid";

        private const string Param_Stoken = "stoken";

        private const string Param_Guid = "guid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            if (!commandLine.TryGetParameter(Param_Guid, out string? guid))
            {
                guid = HoyolabTokenManage.GetGuid();
            }
            HoyolabToken hoyolabToken = new(guid);
            hoyolabToken.SetToken(HoyolabTokenType.SToken, commandLine.GetParameter(Param_Stoken));
            hoyolabToken.Mid = commandLine.GetParameter(Param_Mid);
            return await AsyncInvoke(hoyolabToken, textStream, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabToken hoyolabToken, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            textStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetDeviceFp);
            ITerminalResponse<DeviceFpResponseWrapper> deviceFpResponse = await DeviceFp.AsyncInvoke(cancellationToken).ConfigureAwait(false);
            if (!deviceFpResponse.TryGetContent(out DeviceFpResponseWrapper? deviceFpContent))
            {
                return deviceFpResponse;
            }
            hoyolabToken.Device = deviceFpContent.DeviceFp;
            ExchangeRequestBuilderFactory exchangeFactory = new ExchangeRequestBuilderFactory(hoyolabToken).SetOrigin(HoyolabTokenType.SToken);
            foreach (HoyolabTokenType tokenType in Enum.GetValues<HoyolabTokenType>())
            {
                if (!hoyolabToken.Tokens.ContainsKey(tokenType))
                {
                    exchangeFactory.SetDestin(tokenType);
                    textStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetToken.SafeFormat(tokenType));
                    HttpResponse<ExchangeResponse> exchangeResponse = await exchangeFactory.Create().SendAsync<ExchangeResponse>(cancellationToken).ConfigureAwait(false);
                    if (exchangeResponse.Body.IsNull() || !exchangeResponse.Body.TrySerialize(out ExchangeResponseToken? exchangeToken))
                    {
                        return new TerminalResponse(false, exchangeResponse.ToString());
                    }
                    hoyolabToken.SetToken(tokenType, exchangeToken.Token);
                }
            }
            GameRoleRequestBuilderFactory gameRoleFactory = new(hoyolabToken);
            textStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginGetUserRole);
            HttpResponse<GameRoleResponse> gameRoleResponse = await gameRoleFactory.Create().SendAsync<GameRoleResponse>(cancellationToken).ConfigureAwait(false);
            if (gameRoleResponse.Body.IsNull() || !gameRoleResponse.Body.TrySerialize(out IEnumerable<HoyolabUserRole>? userRoleEnumerable))
            {
                return new TerminalResponse(false, gameRoleResponse.ToString());
            }
            hoyolabToken.UserRoles = [.. userRoleEnumerable];
            textStream?.WriteLine(LocalString.ServiceTerminalHoyolabLoginUserLoginTryUpdate);
            try
            {
                HoyolabTokenManage.Update(hoyolabToken);
            }
            catch (Exception exception)
            {
                return new TerminalResponse(false, exception.Message);
            }
            return new TerminalResponse(true, LocalString.ServiceTerminalHoyolabLoginUserLoginSuccess);
        }
    }
}