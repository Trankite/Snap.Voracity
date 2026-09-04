using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Takumi.Sign;
using Common.Source.Web.Hoyolab.Takumi.Sign.Home;
using Common.Source.Web.Hoyolab.Takumi.Sign.Info;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameSign : AsyncTerminalCommand<SignInfoResponseWrapper>
    {
        public override string Name => "sign";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameSignFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameSignHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [AID];

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            if (!Token.TryGetUserRole(HoyolabApp.StarRailChina.OutSelf(out HoyolabApp Game), out HoyolabUserRole? UserRole))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(HoyolabTerminalResponse.NotFindUserRole(Game));
            }
            SignRequestBody Body = SignRequestBody.Create(UserRole, HoyolabAction.StarRailSign, HoyolabLanguage.Chinese);
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetBody(Body);
            FinalizedResponse<SignResponse> SignActionResponse = await Factory.Create().SendAsync<SignResponse>(cancellationToken);
            if (SignActionResponse.Body.IsNull() || !SignActionResponse.Body.IsSuccess())
            {
                return new TerminalResponse<SignInfoResponseWrapper>(false, SignActionResponse.ToString());
            }
            ITerminalResponse<SignInfoResponseWrapper> SignInfoResponse = await GetSignInfo(Token, UserRole, cancellationToken);
            if (!SignInfoResponse.TryGetAnalyzedBody(out SignInfoResponseWrapper? SignInfo))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(SignInfoResponse);
            }
            ITerminalResponse<SignHomeAnalyzedBody[]> SignAwardsResponse = await GetSignAwards(cancellationToken);
            if (!SignAwardsResponse.TryGetAnalyzedBody(out SignHomeAnalyzedBody[]? SignAwards))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(SignAwardsResponse);
            }
            SignHomeAnalyzedBody? Award = SignAwards.GetIndexValue(SignInfo.TotalSignDay - 1);
            object?[] FormatInfo = [SignInfo.TotalSignDay, SignInfo.TotalSignDay + SignInfo.SignCntMissed, Award?.Name, Award?.Count];
            return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabGameSignContent.SafeFormat(FormatInfo), SignInfo);
        }

        private static async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> GetSignInfo(HoyolabToken hoyolabToken, HoyolabUserRole userRole, CancellationToken cancellationToken = default)
        {
            SignInfoRequestBuilderFactory Factory = new(hoyolabToken, HoyolabAction.StarRailSign, HoyolabLanguage.Chinese, userRole);
            FinalizedResponse<SignInfoResponse> Response = await Factory.Create().SendAsync<SignInfoResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out SignInfoResponseWrapper? AnalyzedBody))
            {
                return TerminalResponse.Create(true, AnalyzedBody);
            }
            return new TerminalResponse<SignInfoResponseWrapper>(false, Response.ToString());
        }

        private static async ValueTask<ITerminalResponse<SignHomeAnalyzedBody[]>> GetSignAwards(CancellationToken cancellationToken = default)
        {
            SignHomeRequestBuilderFactory Factory = new(HoyolabLanguage.Chinese, HoyolabAction.StarRailSign);
            FinalizedResponse<SignHomeResponse> Response = await Factory.Create().SendAsync<SignHomeResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out SignHomeAnalyzedBody[]? AnalyzedBody))
            {
                return TerminalResponse.Create(true, AnalyzedBody);
            }
            return new TerminalResponse<SignHomeAnalyzedBody[]>(false, Response.ToString());
        }
    }
}