using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Hoyolab.Forum
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
            FinalizedResponse<FullPostResponse> Response = await Factory.Create().SendAsync<FullPostResponse>(Program.HttpClient, cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out FullPostResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"[{AnalyedBody.Post.PostId}] {AnalyedBody.Post.Subject}", AnalyedBody);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, Response.ToString());
        }
    }
}