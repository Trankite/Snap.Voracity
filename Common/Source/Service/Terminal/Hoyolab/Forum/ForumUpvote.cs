using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
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

        public override string[] RequiredParameters => [Param_PostId];

        public override string[] OptionalParameters => [Param_IsCancel, Param_Aid];

        private const string Param_PostId = TerminalParameters.Id;

        private const string Param_IsCancel = "cancel";

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_PostId), commandLine.GetBoolParameter(Param_IsCancel), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(string postId, bool isCancel = false, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return SupportTerminalResponse.NotFindToken(aid);
            }
            UpvoteRequestBuilderFactory Factory = new UpvoteRequestBuilderFactory(Token).SetPostId(postId).SetIsCancel(isCancel);
            FinalizedResponse<UpvoteResponse> Response = await Factory.Create().SendAsync<UpvoteResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, StringExtension.SafeFormat(isCancel ? LocalString.ServiceTerminalHoyolabForumUpvoteCancelSuccess : LocalString.ServiceTerminalHoyolabForumUpvoteSuccess, postId));
            }
            return new TerminalResponse(false, Response.ToString());
        }
    }
}