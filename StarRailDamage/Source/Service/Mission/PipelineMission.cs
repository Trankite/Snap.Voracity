using StarRailDamage.Source.Extension;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Mission
{
    public class PipelineMission<TResult> : IQueueMission
    {
        private readonly Func<TResult> Mission;

        private readonly Func<TResult, bool> Predicate = ObjectExtension.IsNotNull;

        public int Attempt { get; set; }

        public int MaxAttempt { get; private set; }

        [MemberNotNullWhen(true, nameof(Result))]
        public bool Success { get; private set; }

        public TResult? Result { get; private set; }

        public PipelineMission(Func<TResult> mission, int maxAttempt = 1)
        {
            Mission = mission;
            MaxAttempt = maxAttempt;
        }

        public PipelineMission(Func<TResult> mission, Func<TResult, bool> predicate, int maxAttempt = 1) : this(mission, maxAttempt)
        {
            Predicate = predicate;
        }

        public virtual bool Invoke()
        {
            return Success = Predicate.Invoke(Result = Mission.Invoke());
        }
    }

    public class PipelineMission<TResult, TPipline> : PipelineMission<TResult>
    {
        private readonly PipelineMission<TPipline> Pipeline;

        public PipelineMission(PipelineMission<TPipline> pipeline, Func<TResult> mission, int maxAttempt = 1) : base(mission, maxAttempt)
        {
            Pipeline = pipeline;
        }

        public PipelineMission(PipelineMission<TPipline> pipeline, Func<TResult> mission, Func<TResult, bool> predicate, int maxAttempt = 1) : base(mission, predicate, maxAttempt)
        {
            Pipeline = pipeline;
        }

        public override bool Invoke()
        {
            return Pipeline.Success ? base.Invoke() : Pipeline.Attempt < Pipeline.MaxAttempt && false.Configure(Pipeline.Attempt = -1);
        }
    }
}