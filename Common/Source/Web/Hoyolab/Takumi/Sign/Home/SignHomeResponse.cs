using Common.Source.Extension;
using Common.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeResponse : ResponseWrapper<SignHomeResponseWrapper, SignHomeAnalyzedBody[]>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out SignHomeAnalyzedBody[]? analyedBody)
        {
            if (TryGetAnalyzedBody(out SignHomeResponseWrapper? Content))
            {
                int Count = Content.Awards.Length;
                analyedBody = new SignHomeAnalyzedBody[Count];
                for (int i = 0; i < Count; i++)
                {
                    SignHomeResponseAward Current = Content.Awards[i];
                    analyedBody[i] = new SignHomeAnalyzedBody(i + 1, Current.Count, Current.Name, Current.Icon);
                }
                return Count > 0;
            }
            return false.Configure(analyedBody = default);
        }
    }
}