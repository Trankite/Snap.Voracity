using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Mission
{
    public class MissionResponse : ResponseWrapper<MissionResponseWrapper, MissionAnalyzedBody>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out MissionAnalyzedBody? analyedBody)
        {
            if (TryGetAnalyzedBody(out MissionResponseWrapper? Content))
            {
                analyedBody = new MissionAnalyzedBody(Content.TotalPoints, Content.TodayTotalPoints, Content.CanGetPoints);
                foreach (MissionResponseState State in Content.States)
                {
                    analyedBody.Mission[(MissionType)State.MissionId] = State.HappenedTimes;
                }
                return true;
            }
            return false.Configure(analyedBody = default);
        }
    }
}