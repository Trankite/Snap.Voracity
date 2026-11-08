using Common.Source.Service.Gradual.Interface;

namespace Common.Source.Service.Gradual
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