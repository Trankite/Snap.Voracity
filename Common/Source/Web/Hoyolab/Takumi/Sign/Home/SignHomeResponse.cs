using Common.Source.Extension;
using Common.Source.Resource.Localization;
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
                int Today = 0;
                analyedBody = new SignHomeAnalyzedBody[Content.Awards.Length];
                foreach (SignHomeResponseAward Award in Content.Awards)
                {
                    analyedBody[Today++] = new SignHomeAnalyzedBody(Today, Award.Count, Award.Name, Award.Icon);
                }
                return true;
            }
            return false.Configure(analyedBody = default);
        }

        public static string GetAwardString(SignHomeAnalyzedBody award)
        {
            return LocalString.WebHoyolabGameSignRewardItem.SafeFormat(award.Today.ToString("D2"), award.Name, award.Count);
        }
    }
}