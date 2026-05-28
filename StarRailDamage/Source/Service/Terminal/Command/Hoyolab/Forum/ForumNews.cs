using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumNews : AsyncTerminalCommand<NewestAnalyzedBody[]>
    {
        public override string Name => "newpost";

        public override string Help => LocalString.ServiceTerminalHoyolabForumNewsHelp;

        public override string[] Parameters => [PAGESIZE, ZONETYPE, SORTTYPE];

        private const string PAGESIZE = "size";

        private const string ZONETYPE = "zone";

        private const string SORTTYPE = "sort";

        protected override async ValueTask<ITerminalResponse<NewestAnalyzedBody[]>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            int PageSize = commandLine.GetIntParameter(PAGESIZE);
            ZoneType ZoneType = (ZoneType)commandLine.GetIntParameter(ZONETYPE);
            SortType SortType = (SortType)commandLine.GetIntParameter(SORTTYPE);
            return await AsyncInvoke(PageSize, ZoneType, SortType);
        }

        public static async ValueTask<ITerminalResponse<NewestAnalyzedBody[]>> AsyncInvoke(int pageSize, ZoneType zoneType, SortType sortType = default)
        {
            NewestRequestBuilderFactory Factory = new NewestRequestBuilderFactory().SetPageSize(pageSize).SetZoneType(zoneType).SetSortType(sortType);
            FinalizedResponse<NewestResponse> Response = await Factory.Create().SendAsync<NewestResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out NewestAnalyzedBody[]? AnalyzedBody))
            {
                if (AnalyzedBody.Length == 0)
                {
                    return new TerminalResponse<NewestAnalyzedBody[]>(TerminalManage.GetUnlawfulParameterResponse());
                }
                return TerminalResponse.Create(true, string.Join('\n', AnalyzedBody.Select(Body => $"[{Body.PostId}] {Body.Title}")), AnalyzedBody);
            }
            return new TerminalResponse<NewestAnalyzedBody[]>(false, Response.ToString());
        }
    }
}