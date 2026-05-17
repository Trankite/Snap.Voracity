using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Web.Hoyolab.Takumi.Sign.Home
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
            return LocalString.WebHoyolabGameSignRewardItem.Format(award.Today.ToString("D2"), award.Name, award.Count);
        }
    }
}