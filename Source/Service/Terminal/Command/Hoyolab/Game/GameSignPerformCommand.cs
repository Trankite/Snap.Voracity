using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Takumi.Sign;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game
{
    public class GameSignPerformCommand : AsyncTerminalCommand<SignResponseWrapper>
    {
        public override string Name => "sign";

        public override string Help => MarkedText.HoyolabGameSignCommandHelp;

        protected override async ValueTask<ITerminalResponse<SignResponseWrapper>> AsyncInvokeOverride(params IList<string> parameter)
        {
            string? AidText = parameter.FirstOrDefault();
            if (!HoyolabTokenManage.TryGetTokenOrFirst(AidText, out HoyolabToken? Token))
            {
                return new TerminalResponse<SignResponseWrapper>(HoyolabTerminalResponse.NotFindToken(AidText));
            }
            if (!Token.TryGetUserRole(GameType.StarRailChina.OutSelf(out GameType Game), out HoyolabUserRole? UserRole))
            {
                return new TerminalResponse<SignResponseWrapper>(HoyolabTerminalResponse.NotFindUserRole(Game));
            }
            SignRequestBody Body = new(HoyolabAction.StarRailSign, UserRole.Server, UserRole.Uid, HoyolabLanguage.ZH_CN);
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetBody(Body);
            FinalizedResponse<SignResponse> Response = await Factory.Create().SendAsync<SignResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return TerminalResponse.Create(true, MarkedText.HoyolabGameSign, Response.Body.Content);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}