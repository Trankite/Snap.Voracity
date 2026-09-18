using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab.Bbs.Forum;
using Common.Source.Web.Hoyolab.Bbs.Forum.Newest;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumNewest : AsyncTerminalCommand<NewestAnalyzedBody[]>
    {
        public override string Name => "newpost";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumNewsFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumNewsHelp;

        public override string[] RequiredParameters => [Param_PageSize, Param_ZoneType];

        public override string[] OptionalParameters => [Param_SortType];

        private const string Param_PageSize = "size";

        private const string Param_ZoneType = "zone";

        private const string Param_SortType = "sort";

        public override async ValueTask<ITerminalResponse<NewestAnalyzedBody[]>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            int PageSize = commandLine.GetIntParameter(Param_PageSize);
            ZoneType ZoneType = (ZoneType)commandLine.GetIntParameter(Param_ZoneType);
            SortType SortType = (SortType)commandLine.GetIntParameter(Param_SortType);
            return await AsyncInvoke(PageSize, ZoneType, SortType, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<NewestAnalyzedBody[]>> AsyncInvoke(int pageSize, ZoneType zoneType, SortType sortType = default, CancellationToken cancellationToken = default)
        {
            NewestRequestBuilderFactory Factory = new NewestRequestBuilderFactory().SetPageSize(pageSize).SetZoneType(zoneType).SetSortType(sortType);
            FinalizedResponse<NewestResponse> Response = await Factory.Create().SendAsync<NewestResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out NewestAnalyzedBody[]? AnalyzedBody))
            {
                if (AnalyzedBody.Length == 0)
                {
                    return new TerminalResponse<NewestAnalyzedBody[]>(SupportTerminalResponse.UnlawfulParameter());
                }
                return TerminalResponse.Create(true, string.Join('\n', AnalyzedBody.Select(Body => $"[{Body.PostId}] {Body.Title}")), AnalyzedBody);
            }
            return new TerminalResponse<NewestAnalyzedBody[]>(false, Response.ToString());
        }
    }
}