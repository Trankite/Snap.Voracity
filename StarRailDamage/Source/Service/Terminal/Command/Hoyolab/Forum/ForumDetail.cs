using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumDetail : AsyncTerminalCommand<FullPostResponseWrapper>
    {
        public override string Name => "post";

        public override string Help => LocalString.ServiceTerminalHoyolabForumDetailHelp;

        public override string[] Parameters => [POSTID, NEEDSIGN, AID];

        private const string POSTID = "id";

        private const string NEEDSIGN = "sign";

        private const string AID = "aid";

        protected override async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetParameter(POSTID, out string PostId))
            {
                return new TerminalResponse<FullPostResponseWrapper>(TerminalManage.GetMissingParameterResponse());
            }
            return await AsyncInvoke(PostId, commandLine.GetBoolParameter(NEEDSIGN), commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvoke(string postId, bool needSign = false, string? aid = null)
        {
            HoyolabToken? Token = null;
            if (needSign && !HoyolabTokenManage.TryGetTokenOrFirst(aid, out Token))
            {
                return new TerminalResponse<FullPostResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            if (Token.IsNull())
            {
                Token = new HoyolabToken();
            }
            FullPostRequestBuilderFactory Factory = new FullPostRequestBuilderFactory(Token).SetPostId(postId);
            FinalizedResponse<FullPostResponse> Response = await Factory.Create().SendAsync<FullPostResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out FullPostResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"[{AnalyedBody.Post.PostId}] {AnalyedBody.Post.Subject}", AnalyedBody);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, Response.ToString());
        }
    }
}