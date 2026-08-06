using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Upvote;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumUpvote : AsyncTerminalCommand
    {
        public override string Name => "upvote";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumUpvoteFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumUpvoteHelp;

        public override string[] RequiredParameters => [POSTID];

        public override string[] OptionalParameters => [ISCANCEL, AID];

        private const string POSTID = "id";

        private const string ISCANCEL = "cancel";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(POSTID), commandLine.GetBoolParameter(ISCANCEL), commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string postId, bool isCancel = false, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return HoyolabTerminalResponse.NotFindToken(aid);
            }
            UpvoteRequestBuilderFactory Factory = new UpvoteRequestBuilderFactory(Token).SetPostId(postId).SetIsCancel(isCancel);
            FinalizedResponse<UpvoteResponse> Response = await Factory.Create().SendAsync<UpvoteResponse>(Program.HttpClient, cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, StringExtension.Format(isCancel ? LocalString.WebHoyolabForumUpvoteCancelSuccess : LocalString.WebHoyolabForumUpvoteSuccess, postId));
            }
            return new TerminalResponse(false, Response.ToString());
        }
    }
}