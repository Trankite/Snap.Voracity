using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Takumi.Sign;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameSign : AsyncTerminalCommand<SignResponseWrapper>
    {
        public override string Name => "sign";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameSignFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameSignHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [AID];

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<SignResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<SignResponseWrapper>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<SignResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            if (!Token.TryGetUserRole(HoyolabApp.StarRailChina.OutSelf(out HoyolabApp Game), out HoyolabUserRole? UserRole))
            {
                return new TerminalResponse<SignResponseWrapper>(HoyolabTerminalResponse.NotFindUserRole(Game));
            }
            SignRequestBody Body = SignRequestBody.Create(HoyolabAction.StarRailSign, UserRole.Server, UserRole.Uid, HoyolabLanguage.Chinese);
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetBody(Body);
            FinalizedResponse<SignResponse> Response = await Factory.Create().SendAsync<SignResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return TerminalResponse.Create(true, LocalString.WebHoyolabGameSignSuccess, Response.Body.Content);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}