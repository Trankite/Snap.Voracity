using Common.Source.Extension;
using Common.Source.Service.Streams.Text.Abstract;
using Common.Source.Service.Streams.Text.Interface;
using Common.Source.Service.Terminal.Interface;
using System.Diagnostics;

namespace Common.Source.Service.Terminal.Abstract
{
    public static class TerminalResponseExtension
    {
        [DebuggerStepThrough]
        public static T WriteTo<T>(this T response, ITextStream? textStream) where T : ITerminalResponse
        {
            if (textStream.IsNotNull())
            {
                textStream.WriteLine(response.Message);
            }
            return response;
        }
    }
}