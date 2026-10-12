using Common.Source.Extension;
using Common.Source.Factory.Serialize;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
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

        public override string[] RequiredParameters => [Param_PostId];

        public override string[] OptionalParameters => [Param_NeedSign, Param_Aid];

        private const string Param_PostId = TerminalParameters.Id;

        private const string Param_NeedSign = "sign";

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_PostId), commandLine.GetBoolParameter(Param_NeedSign), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvoke(string postId, bool needSign = false, string? aid = default, CancellationToken cancellationToken = default)
        {
            HoyolabToken? Token = default;
            if (needSign && !HoyolabTokenManage.TryGetTokenOrFirst(aid, out Token))
            {
                return new TerminalResponse<FullPostResponseWrapper>(SupportTerminalResponse.NotFindToken(aid));
            }
            Token ??= new HoyolabToken();
            FullPostRequestBuilderFactory Factory = new FullPostRequestBuilderFactory(Token).SetPostId(postId);
            HttpResponse<FullPostResponse> Response = await Factory.Create().SendAsync<FullPostResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TrySerialize(out FullPostResponseWrapper? FullPost))
            {
                return TerminalResponse.Create(true, $"[{FullPost.Post.PostId}]\x20{FullPost.Post.Subject}", FullPost);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, Response.ToString());
        }
    }
}