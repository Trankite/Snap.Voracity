using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Mission
{
    public class MissionResponse : ResponseWrapper<MissionResponseWrapper>, IContentSerializer<MissionInfo?>
    {
        public MissionInfo? Serialize()
        {
            MissionInfo? MissionInfo = default;
            if (TryGetContent(out MissionResponseWrapper? Content))
            {
                MissionInfo = new MissionInfo(Content.TotalPoints, Content.AlreadyReceivedPoints, Content.CanGetPoints);
                foreach (MissionResponseState State in Content.States)
                {
                    MissionInfo.Mission[(MissionType)State.MissionId] = State.HappenedTimes;
                }
            }
            return MissionInfo;
        }
    }
}