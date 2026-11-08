using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Resource.Localization;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumDetail : AsyncTerminalCommand<FullPostResponseWrapper>
    {
        public override string Name => "post";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumDetailFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumDetailHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_PostId];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_NeedSign, Param_Aid];

        private const string Param_PostId = TerminalParameters.Id;

        private const string Param_NeedSign = "sign";

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_PostId), commandLine.GetBoolParameter(Param_NeedSign), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvoke(string postId, bool needSign = false, string? aid = default, CancellationToken cancellationToken = default)
        {
            HoyolabToken? hoyolabToken = default;
            if (needSign && !HoyolabTokenManage.TryGetTokenOrFirst(aid, out hoyolabToken))
            {
                return new TerminalResponse<FullPostResponseWrapper>(SupportTerminalResponse.NotFindToken(aid));
            }
            hoyolabToken ??= new HoyolabToken();
            FullPostRequestBuilderFactory factory = new FullPostRequestBuilderFactory(hoyolabToken).SetPostId(postId);
            HttpResponse<FullPostResponse> response = await factory.Create().SendAsync<FullPostResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TrySerialize(out FullPostResponseWrapper? fullPost))
            {
                return TerminalResponse.Create(true, $"[{fullPost.Post.PostId}]\x20{fullPost.Post.Subject}", fullPost);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, response.ToString());
        }
    }
}