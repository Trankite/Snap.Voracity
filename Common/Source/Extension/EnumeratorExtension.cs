using Common.Source.Model.DataStruct.Ticket;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Extension
{
    public static class EnumeratorExtension
    {
        [DebuggerStepThrough]
        public static bool TryMoveNext<T>(this IEnumerator<T> enumerator, [NotNullWhen(true)] out T? value)
        {
            return enumerator.MoveNext() ? true.Configure(value = enumerator.Current) : false.Configure(value = default);
        }

        [DebuggerStepThrough]
        public static async ValueTask<CheckTicket<T>> MoveAsync<T>(this IAsyncEnumerator<T> asyncEnumerator)
        {
            return await asyncEnumerator.MoveNextAsync().ConfigureAwait(false) ? CheckTicket.Create(asyncEnumerator.Current) : default;
        }
    }
}