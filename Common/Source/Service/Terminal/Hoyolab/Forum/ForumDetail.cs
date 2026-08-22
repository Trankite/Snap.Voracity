using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumDetail : AsyncTerminalCommand<FullPostResponseWrapper>
    {
        public override string Name => "post";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumDetailFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumDetailHelp;

        public override string[] RequiredParameters => [POSTID];

        public override string[] OptionalParameters => [NEEDSIGN, AID];

        private const string POSTID = "id";

        private const string NEEDSIGN = "sign";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(POSTID), commandLine.GetBoolParameter(NEEDSIGN), commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvoke(string postId, bool needSign = false, string? aid = default, CancellationToken cancellationToken = default)
        {
            HoyolabToken? Token = default;
            if (needSign && !HoyolabTokenManage.TryGetTokenOrFirst(aid, out Token))
            {
                return new TerminalResponse<FullPostResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            if (Token.IsNull())
            {
                Token = new HoyolabToken();
            }
            FullPostRequestBuilderFactory Factory = new FullPostRequestBuilderFactory(Token).SetPostId(postId);
            FinalizedResponse<FullPostResponse> Response = await Factory.Create().SendAsync<FullPostResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out FullPostResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"[{AnalyedBody.Post.PostId}] {AnalyedBody.Post.Subject}", AnalyedBody);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, Response.ToString());
        }
    }
}