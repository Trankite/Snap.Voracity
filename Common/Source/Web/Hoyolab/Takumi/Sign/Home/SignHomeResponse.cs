using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeResponse : ResponseWrapper<SignHomeResponseWrapper>, IContentSerializer<IEnumerable<SignHomeAnalyzedBody>?>
    {
        public IEnumerable<SignHomeAnalyzedBody>? Serialize()
        {
            return TryGetContent(out SignHomeResponseWrapper? Content) ? Content.Awards.Select((Current, Index) => new SignHomeAnalyzedBody(Index + 1, Current.Count, Current.Name, Current.Icon)) : default;
        }
    }
}