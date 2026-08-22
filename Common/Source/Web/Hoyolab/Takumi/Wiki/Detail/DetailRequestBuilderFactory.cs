using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.Takumi.Wiki.Detail;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class DetailRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://act-api-takumi-static.mihoyo.com/common/blackboard/sr_wiki/v1/content/info";

        public int ContentId { get; set; }

        public DetailRequestBuilderFactory() { }

        public HttpRequestMessageBuilder Create()
        {
            return new HttpRequestMessageBuilder().SetRequestUri(new HoyolabHttpUriBuilder(URL).SetChannalId(HoyolabChannal.StarRailWiki).SetContentId(ContentId.ToString()));
        }
    }
}