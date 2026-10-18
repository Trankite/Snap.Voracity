using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeResponse : ResponseWrapper<SignHomeResponseWrapper>, IContentSerializer<IEnumerable<SignHomeAnalyzedBody>?>
    {
        public IEnumerable<SignHomeAnalyzedBody>? Serialize()
        {
            return TryGetContent(out SignHomeResponseWrapper? Content) ? Content.Awards.Select((current, index) => new SignHomeAnalyzedBody(index + 1, current.Count, current.Name, current.Icon)) : default;
        }
    }
}