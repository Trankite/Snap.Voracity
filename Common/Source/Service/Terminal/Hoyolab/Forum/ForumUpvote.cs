using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Forum.Upvote;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
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
            FinalizedResponse<UpvoteResponse> Response = await Factory.Create().SendAsync<UpvoteResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, StringExtension.SafeFormat(isCancel ? LocalString.WebHoyolabForumUpvoteCancelSuccess : LocalString.WebHoyolabForumUpvoteSuccess, postId));
            }
            return new TerminalResponse(false, Response.ToString());
        }
    }
}