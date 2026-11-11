using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class TaskExtension
    {
        [DebuggerStepThrough]
        public static void GetAwaitResult(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        [DebuggerStepThrough]
        public static T GetAwaitResult<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        [DebuggerStepThrough]
        public static void GetAwaitResult(this ValueTask valueTask)
        {
            valueTask.GetAwaiter().GetResult();
        }

        [DebuggerStepThrough]
        public static T GetAwaitResult<T>(this ValueTask<T> valueTask)
        {
            return valueTask.GetAwaiter().GetResult();
        }
    }
}