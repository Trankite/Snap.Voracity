using System.Diagnostics;

namespace Common.Source.Factory.Streams.Html.Metadata
{
    public static class HtmlStatusExtension
    {
        [DebuggerStepThrough]
        public static bool IsCreated(this HtmlStatus htmlStatus)
        {
            return htmlStatus == HtmlStatus.Created;
        }

        [DebuggerStepThrough]
        public static bool IsNested(this HtmlStatus htmlStatus)
        {
            return htmlStatus == HtmlStatus.Nested;
        }

        [DebuggerStepThrough]
        public static bool IsCompleted(this HtmlStatus htmlStatus)
        {
            return htmlStatus == HtmlStatus.Completed;
        }
    }
}