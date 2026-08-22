using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
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

        public override string[] RequiredParameters => [POSTID];

        public override string[] OptionalParameters => [AID];

        private const string POSTID = "id";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(POSTID), commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvoke(string postId, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<ShareResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            ShareRequestBuilderFactory Factory = new ShareRequestBuilderFactory(Token).SetEntityType(EntityType.Post).SetEntityId(postId);
            FinalizedResponse<ShareResponse> Response = await Factory.Create().SendAsync<ShareResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out ShareResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"{AnalyedBody.Title}\n{AnalyedBody.Url}", AnalyedBody);
            }
            return new TerminalResponse<ShareResponseWrapper>(false, Response.ToString());
        }
    }
}