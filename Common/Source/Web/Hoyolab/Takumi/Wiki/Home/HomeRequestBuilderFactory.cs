using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Home
{
    public class HomeRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        public ChannelType ChannelType { get; set; }

        private const string URL = "https://act-api-takumi-static.mihoyo.com/common/blackboard/sr_wiki/v1/home/content/list";

        public HomeRequestBuilderFactory() { }

        public HttpRequestMessageBuilder Create()
        {
            return new HttpRequestMessageBuilder().SetRequestUri(new HoyolabHttpUriBuilder(URL).SetChannalId(HoyolabChannal.StarRailWiki).SetAppSn(ChannelType.ToIntString()));
        }
    }
}