using Common.Source.Extension;
using Common.Source.Web.Response;
using Common.Source.Web.Response.Wrapper;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Home
{
    public sealed class HomeResponse : ResponseWrapper<ListWrapper<HomeResponseWrapper>>
    {
        public IEnumerator<HomeResponseCycle> GetEnumerator()
        {
            if (Content.IsNull()) yield break;
            foreach (HomeResponseWrapper homeResponse in Content.List)
            {
                yield return GetHomeContentCycle(homeResponse);
            }
        }

        private static HomeResponseCycle GetHomeContentCycle(HomeResponseWrapper homeResponse)
        {
            HomeResponseCycle homeResponseCycle = new(homeResponse.Name, homeResponse.List);
            foreach (HomeResponseWrapper homeContentChildren in homeResponse.Children)
            {
                homeResponseCycle.Children.Add(GetHomeContentCycle(homeContentChildren));
            }
            return homeResponseCycle;
        }
    }
}