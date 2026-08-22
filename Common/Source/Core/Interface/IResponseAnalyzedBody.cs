using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Core.Interface
{
    public interface IResponseAnalyzedBody<TContent>
    {
        bool TryGetAnalyzedBody([NotNullWhen(true)] out TContent? analyedBody);
    }
}