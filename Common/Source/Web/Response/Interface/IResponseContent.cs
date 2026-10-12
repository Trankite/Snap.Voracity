using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Response.Interface
{
    public interface IResponseContent<TContent>
    {
        TContent? Content { get; }

        bool TryGetContent([NotNullWhen(true)] out TContent? content);
    }
}