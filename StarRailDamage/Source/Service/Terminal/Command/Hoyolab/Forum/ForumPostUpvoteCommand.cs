using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Upvote;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumPostUpvoteCommand : AsyncTerminalCommand
    {
        public override string Name => "upvote";

        public override string Help => MarkedText.HoyolabPostUpvoteCommandHelp;

        public override string[] Parameters => [POSTID, ISCANCEL, AID];

        private const string POSTID = "id";

        private const string ISCANCEL = "cancel";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetParameter(POSTID, out string? PostId))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            return await AsyncInvoke(PostId, commandLine.GetBoolParameter(ISCANCEL), commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string postId, bool isCancel = false, string? aid = null)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return HoyolabTerminalResponse.NotFindToken(aid);
            }
            UpvoteRequestBuilderFactory Factory = new UpvoteRequestBuilderFactory(Token).SetPostId(postId).SetIsCancel(isCancel);
            FinalizedResponse<UpvoteResponse> Response = await Factory.Create().SendAsync<UpvoteResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, StringExtension.Format(isCancel ? MarkedText.HoyolabForumPostUpvoteCancel : MarkedText.HoyolabForumPostUpvote, postId));
            }
            return new TerminalResponse(false, Response.ToString());
        }
    }
}