using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Takumi.Sign.Home;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameSignReward : AsyncTerminalCommand<SignHomeAnalyzedBody[]>
    {
        public override string Name => "signer";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameSignRewardFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameSignRewardHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [START, TOTAL];

        private const string START = "start";

        private const string TOTAL = "total";

        public override async ValueTask<ITerminalResponse<SignHomeAnalyzedBody[]>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetIntParameter(START), commandLine.GetIntParameter(TOTAL), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<SignHomeAnalyzedBody[]>> AsyncInvoke(int start = 0, int total = 0, CancellationToken cancellationToken = default)
        {
            SignHomeRequestBuilderFactory Factory = new(HoyolabLanguage.Chinese, HoyolabAction.StarRailSign);
            FinalizedResponse<SignHomeResponse> Response = await Factory.Create().SendAsync<SignHomeResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out SignHomeAnalyzedBody[]? Body))
            {
                int Index = Body.ClampIndex(start - 1);
                int Count = Body.ClampCount(Index, total > 0 ? total : Body.Length);
                SignHomeAnalyzedBody[] FindArray = new SignHomeAnalyzedBody[Count].Configure(Self => Array.Copy(Body, Index, Self, 0, Count));
                return TerminalResponse.Create(true, string.Join('\n', FindArray.Select(SignHomeResponse.GetAwardString)), FindArray);
            }
            return new TerminalResponse<SignHomeAnalyzedBody[]>(false, Response.ToString());
        }
    }
}