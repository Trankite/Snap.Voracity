using Common.Source.Extension;
using Common.Source.Factory.Serialize;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Web.Hoyolab.Bbs.Forum;
using Common.Source.Web.Hoyolab.Bbs.Forum.Newest;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumNewest : AsyncTerminalCommand<IEnumerable<NewestAnalyzedBody>>
    {
        public override string Name => "newpost";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumNewsFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumNewsHelp;

        public override string[] RequiredParameters => [Param_PageSize, Param_ZoneType];

        public override string[] OptionalParameters => [Param_SortType];

        private const string Param_PageSize = "size";

        private const string Param_ZoneType = "zone";

        private const string Param_SortType = "sort";

        public override async ValueTask<ITerminalResponse<IEnumerable<NewestAnalyzedBody>>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            int PageSize = commandLine.GetIntParameter(Param_PageSize);
            ZoneType ZoneType = (ZoneType)commandLine.GetIntParameter(Param_ZoneType);
            SortType SortType = (SortType)commandLine.GetIntParameter(Param_SortType);
            return await AsyncInvoke(PageSize, ZoneType, SortType, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<IEnumerable<NewestAnalyzedBody>>> AsyncInvoke(int pageSize, ZoneType zoneType, SortType sortType = default, CancellationToken cancellationToken = default)
        {
            NewestRequestBuilderFactory Factory = new NewestRequestBuilderFactory().SetPageSize(pageSize).SetZoneType(zoneType).SetSortType(sortType);
            HttpResponse<NewestResponse> Response = await Factory.Create().SendAsync<NewestResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TrySerialize(out IEnumerable<NewestAnalyzedBody>? AnalyzedBody))
            {
                return TerminalResponse.Create(true, string.Join('\n', AnalyzedBody.Select(Current => $"[{Current.PostId}]\x20{Current.Title}")), AnalyzedBody);
            }
            return new TerminalResponse<IEnumerable<NewestAnalyzedBody>>(false, Response.ToString());
        }
    }
}