using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Mission
{
    public class MissionResponse : ResponseWrapper<MissionResponseWrapper>, IContentSerializer<MissionInfo?>
    {
        public MissionInfo? Serialize()
        {
            MissionInfo? missionInfo = default;
            if (TryGetContent(out MissionResponseWrapper? content))
            {
                missionInfo = new MissionInfo(content.TotalPoints, content.AlreadyReceivedPoints, content.CanGetPoints);
                foreach (MissionResponseState state in content.States)
                {
                    missionInfo.Mission[(MissionType)state.MissionId] = state.HappenedTimes;
                }
            }
            return missionInfo;
        }
    }
}