using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Takumi.Sign.Home;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game
{
    public class GameSignRewardCommand : AsyncTerminalCommand<SignHomeAnalyzedBody[]>
    {
        public override string Name => "signer";

        public override string Help => MarkedText.HoyolabGameSignRewardCommandHelp;

        protected override async ValueTask<ITerminalResponse<SignHomeAnalyzedBody[]>> AsyncInvokeOverride(params IList<string> parameter)
        {
            SignHomeRequestBuilderFactory Factory = new(HoyolabLanguage.ZH_CN, HoyolabAction.StarRailSign);
            FinalizedResponse<SignHomeResponse> Response = await Factory.Create().SendAsync<SignHomeResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out SignHomeAnalyzedBody[]? Body))
            {
                int InputCount = IntExtension.Parse(parameter.Index(1));
                int Index = CollectionExtension.AutoIndex(IntExtension.Parse(parameter.Index(0)) - 1, Body.Length);
                int Count = CollectionExtension.AutoCount(Index, InputCount > 0 ? InputCount : Body.Length, Body.Length);
                SignHomeAnalyzedBody[] FindArray = new SignHomeAnalyzedBody[Count].Configure(Self => Array.Copy(Body, Index, Self, 0, Count));
                return TerminalResponse.Create(true, string.Join('\n', FindArray.Select(SignHomeResponse.GetAwardString)), FindArray);
            }
            return new TerminalResponse<SignHomeAnalyzedBody[]>(false, Response.ToString());
        }
    }
}