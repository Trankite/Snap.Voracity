using System.Diagnostics;

namespace Common.Source.Service.Mission.Gradual.Metadata
{
    public static class GradualStatusExtension
    {
        [DebuggerStepThrough]
        public static bool IsCreated(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Created;
        }

        [DebuggerStepThrough]
        public static bool IsRunning(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Running;
        }

        [DebuggerStepThrough]
        public static bool IsCompleted(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Completed;
        }

        [DebuggerStepThrough]
        public static bool IsCanceled(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Canceled;
        }

        [DebuggerStepThrough]
        public static bool IsFaulted(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Faulted;
        }

        [DebuggerStepThrough]
        public static bool IsRunnable(this GradualStatus gradualStatus)
        {
            return gradualStatus == GradualStatus.Created || gradualStatus == GradualStatus.Running;
        }
    }
}