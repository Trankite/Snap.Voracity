using System.Diagnostics;

namespace Common.Source.Service.Mission.Gradual.Metadata
{
    public static class GradualStatesExtension
    {
        [DebuggerStepThrough]
        public static bool IsUnCompleted(this GradualStates gradualStates)
        {
            return gradualStates == GradualStates.Created || gradualStates == GradualStates.Suspend;
        }
    }
}