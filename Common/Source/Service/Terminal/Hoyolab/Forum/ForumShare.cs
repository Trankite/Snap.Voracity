using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Forum.Share;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumShare : AsyncTerminalCommand<ShareResponseWrapper>
    {
        public override string Name => "share";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumShareFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumShareHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_PostId];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Aid];

        private const string Param_PostId = TerminalParameters.Id;

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_PostId), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvoke(string postId, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? hoyolabToken))
            {
                return new TerminalResponse<ShareResponseWrapper>(SupportTerminalResponse.NotFindToken(aid));
            }
            ShareRequestBuilderFactory factory = new ShareRequestBuilderFactory(hoyolabToken).SetEntityType(EntityType.Post).SetEntityId(postId);
            HttpResponse<ShareResponse> response = await factory.Create().SendAsync<ShareResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TryGetContent(out ShareResponseWrapper? shareInfo))
            {
                return TerminalResponse.Create(true, $"{shareInfo.Title}\n{shareInfo.Url}", shareInfo);
            }
            return new TerminalResponse<ShareResponseWrapper>(false, response.ToString());
        }
    }
}