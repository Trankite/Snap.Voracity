using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Model.DataStruct.Ticket
{
    public static class CheckTicketExtension
    {
        [DebuggerStepThrough]
        public static bool TryGetValue<T>(this CheckTicket<T> checkTicket, [NotNullWhen(true)] out T? value)
        {
            return checkTicket.Success.Configure(value = checkTicket.Ticket);
        }
    }
}