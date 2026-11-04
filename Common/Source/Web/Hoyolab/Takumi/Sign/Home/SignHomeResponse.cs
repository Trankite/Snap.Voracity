using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeResponse : ResponseWrapper<SignHomeResponseWrapper>, IContentSerializer<IEnumerable<SignAward>?>
    {
        public IEnumerable<SignAward>? Serialize()
        {
            return TryGetContent(out SignHomeResponseWrapper? content) ? content.Awards.Select((current, index) => new SignAward(index + 1, current.Count, current.Name, current.Icon)) : default;
        }
    }
}