using Common.Source.Service.Mission.Gradual.Interface;

namespace Common.Source.Service.Mission.Gradual
{
    public sealed class GradualRefresher : IDisposable
    {
        private readonly IGradualTask GradualTask;

        public GradualRefresher(IGradualTask gradualTask)
        {
            GradualTask = gradualTask;
        }

        public void Dispose()
        {
            GradualTask.Refresh();
        }
    }
}