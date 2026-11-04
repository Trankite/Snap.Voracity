using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Web.Hoyolab.Bbs.Forum;
using Common.Source.Web.Hoyolab.Bbs.Forum.Newest;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumNewest : AsyncTerminalCommand<IEnumerable<NewestPost>>
    {
        public override string Name => "newpost";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumNewsFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumNewsHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_PageSize, Param_ZoneType];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_SortType];

        private const string Param_PageSize = "size";

        private const string Param_ZoneType = "zone";

        private const string Param_SortType = "sort";

        public override async ValueTask<ITerminalResponse<IEnumerable<NewestPost>>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            int pageSize = commandLine.GetIntParameter(Param_PageSize);
            ZoneType zoneType = (ZoneType)commandLine.GetIntParameter(Param_ZoneType);
            SortType sortType = (SortType)commandLine.GetIntParameter(Param_SortType);
            return await AsyncInvoke(pageSize, zoneType, sortType, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<IEnumerable<NewestPost>>> AsyncInvoke(int pageSize, ZoneType zoneType, SortType sortType = default, CancellationToken cancellationToken = default)
        {
            NewestRequestBuilderFactory factory = new NewestRequestBuilderFactory().SetPageSize(pageSize).SetZoneType(zoneType).SetSortType(sortType);
            HttpResponse<NewestResponse> response = await factory.Create().SendAsync<NewestResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TrySerialize(out IEnumerable<NewestPost>? newestPost))
            {
                return TerminalResponse.Create(true, string.Join('\n', newestPost.Select(current => $"[{current.PostId}]\x20{current.Title}")), newestPost);
            }
            return new TerminalResponse<IEnumerable<NewestPost>>(false, response.ToString());
        }
    }
}