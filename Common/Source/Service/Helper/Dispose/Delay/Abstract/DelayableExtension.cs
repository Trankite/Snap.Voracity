using Common.Source.Extension;
using Common.Source.Service.Helper.Dispose.Delay.Interface;
using System.Diagnostics;

namespace Common.Source.Service.Helper.Dispose.Delay.Abstract
{
    public static class DelayableExtension
    {
        [DebuggerStepThrough]
        public static T SetDelayer<T>(this T delayable, IDelayer delayer) where T : IDelayable
        {
            return delayable.Configure(delayable.Delayer = delayer);
        }
    }
}