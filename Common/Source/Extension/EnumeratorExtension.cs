using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Extension
{
    public static class EnumeratorExtension
    {
        [DebuggerStepThrough]
        public static bool TryGetNext<T>(this IEnumerator<T> enumerator, [NotNullWhen(true)] out T? value) where T : allows ref struct
        {
            return enumerator.MoveNext() ? true.Configure(value = enumerator.Current) : false.Configure(value = default);
        }
    }
}