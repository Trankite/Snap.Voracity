using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/painter/api/getRecentForumPostList";

        public ZoneType ZoneType { get; set; }

        public SortType SortType { get; set; }

        public int PageSize { get; set; }

        public NewestRequestBuilderFactory() { }

        public HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder().SetRequestUri(new HoyolabHttpUriBuilder(URL).SetForumId(ZoneType).SetSortType(SortType).SetPageSize(PageSize));
        }
    }
}