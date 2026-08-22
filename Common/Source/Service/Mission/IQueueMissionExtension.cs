using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Service.Mission
{
    public static class IQueueMissionExtension
    {
        [DebuggerStepThrough]
        public static MissionQueue Create(this IQueueMission mission)
        {
            return new MissionQueue().Configure(Self => Self.Queue.Enqueue(mission));
        }

        [DebuggerStepThrough]
        public static MissionQueue Create(params IQueueMission[] missions)
        {
            return new MissionQueue(new Queue<IQueueMission>(missions));
        }
    }
}