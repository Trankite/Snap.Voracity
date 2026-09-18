using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Forum.Share;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumShare : AsyncTerminalCommand<ShareResponseWrapper>
    {
        public override string Name => "share";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumShareFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumShareHelp;

        public override string[] RequiredParameters => [Param_PostId];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_PostId = TerminalParameters.Id;

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_PostId), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvoke(string postId, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<ShareResponseWrapper>(SupportTerminalResponse.NotFindToken(aid));
            }
            ShareRequestBuilderFactory Factory = new ShareRequestBuilderFactory(Token).SetEntityType(EntityType.Post).SetEntityId(postId);
            FinalizedResponse<ShareResponse> Response = await Factory.Create().SendAsync<ShareResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out ShareResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"{AnalyedBody.Title}\n{AnalyedBody.Url}", AnalyedBody);
            }
            return new TerminalResponse<ShareResponseWrapper>(false, Response.ToString());
        }
    }
}