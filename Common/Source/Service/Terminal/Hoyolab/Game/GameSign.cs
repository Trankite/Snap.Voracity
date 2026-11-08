using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Takumi.Sign;
using Common.Source.Web.Hoyolab.Takumi.Sign.Home;
using Common.Source.Web.Hoyolab.Takumi.Sign.Info;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameSign : AsyncTerminalCommand<SignInfoResponseWrapper>
    {
        public override string Name => "sign";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameSignFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameSignHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? hoyolabToken))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(SupportTerminalResponse.NotFindToken(aid));
            }
            if (!hoyolabToken.TryGetUserRole(HoyolabApp.StarRailChina.OutSender(out HoyolabApp hoyolabApp), out HoyolabUserRole? userRole))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(HoyolabTerminalResponse.NotFindUserRole(hoyolabApp));
            }
            SignRequestBody body = SignRequestBody.Create(userRole, HoyolabAction.StarRailSign, HoyolabLanguage.Chinese);
            SignRequestBuilderFactory factory = new SignRequestBuilderFactory(hoyolabToken).SetBody(body);
            HttpResponse<SignResponse> signActionResponse = await factory.Create().SendAsync<SignResponse>(cancellationToken).ConfigureAwait(false);
            if (signActionResponse.Body.IsNull() || !signActionResponse.Body.Success)
            {
                return new TerminalResponse<SignInfoResponseWrapper>(false, signActionResponse.ToString());
            }
            ITerminalResponse<SignInfoResponseWrapper> signInfoResponse = await GetSignInfo(hoyolabToken, userRole, cancellationToken).ConfigureAwait(false);
            if (!signInfoResponse.TryGetContent(out SignInfoResponseWrapper? signInfo))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(signInfoResponse);
            }
            ITerminalResponse<IEnumerable<SignAward>> signAwardsResponse = await GetSignAwards(cancellationToken).ConfigureAwait(false);
            if (!signAwardsResponse.TryGetContent(out IEnumerable<SignAward>? signAwards))
            {
                return new TerminalResponse<SignInfoResponseWrapper>(signAwardsResponse);
            }
            SignAward? award = signAwards.ToArray().GetIndexValue(signInfo.TotalSignDay - 1);
            object?[] formatInfo = [signInfo.TotalSignDay, signInfo.TotalSignDay + signInfo.SignCntMissed, award?.Name, award?.Count];
            return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabGameSignContent.SafeFormat(formatInfo), signInfo);
        }

        private static async ValueTask<ITerminalResponse<SignInfoResponseWrapper>> GetSignInfo(HoyolabToken hoyolabToken, HoyolabUserRole userRole, CancellationToken cancellationToken = default)
        {
            SignInfoRequestBuilderFactory factory = new(hoyolabToken, HoyolabAction.StarRailSign, HoyolabLanguage.Chinese, userRole);
            HttpResponse<SignInfoResponse> response = await factory.Create().SendAsync<SignInfoResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TryGetContent(out SignInfoResponseWrapper? signInfo))
            {
                return TerminalResponse.Create(true, signInfo);
            }
            return new TerminalResponse<SignInfoResponseWrapper>(false, response.ToString());
        }

        private static async ValueTask<ITerminalResponse<IEnumerable<SignAward>>> GetSignAwards(CancellationToken cancellationToken = default)
        {
            SignHomeRequestBuilderFactory factory = new(HoyolabLanguage.Chinese, HoyolabAction.StarRailSign);
            HttpResponse<SignHomeResponse> response = await factory.Create().SendAsync<SignHomeResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TrySerialize(out IEnumerable<SignAward>? signAward))
            {
                return TerminalResponse.Create(true, signAward);
            }
            return new TerminalResponse<IEnumerable<SignAward>>(false, response.ToString());
        }
    }
}