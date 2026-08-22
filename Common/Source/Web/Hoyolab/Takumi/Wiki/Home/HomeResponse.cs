using Common.Source.Extension;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Home
{
    public sealed class HomeResponse : ResponseWrapper<ListWrapper<HomeResponseWrapper>>
    {
        public IEnumerator<HomeResponseCycle> GetEnumerator()
        {
            if (Content.IsNull()) yield break;
            foreach (HomeResponseWrapper HomeResponseData in Content.List)
            {
                yield return GetHomeContentCycle(HomeResponseData);
            }
        }

        private static HomeResponseCycle GetHomeContentCycle(HomeResponseWrapper homeResponseData)
        {
            HomeResponseCycle HomeResponseCycle = new(homeResponseData.Name, homeResponseData.List);
            foreach (HomeResponseWrapper HomeContentDataChildren in homeResponseData.Children)
            {
                HomeResponseCycle.Children.Add(GetHomeContentCycle(HomeContentDataChildren));
            }
            return HomeResponseCycle;
        }
    }
}