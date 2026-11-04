using Common.Source.Extension;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Model.DataStruct.Ticket
{
    public static class CheckTicket
    {
        public static CheckTicket<T> Create<T>(T ticket)
        {
            return Create(true, ticket);
        }

        public static CheckTicket<T> Create<T>(bool success, T? ticket = default)
        {
            return new CheckTicket<T>(success, ticket);
        }
    }

    public readonly struct CheckTicket<T>
    {
        public T? Ticket { get; }

        [MemberNotNullWhen(true, nameof(Ticket))]
        public bool Success { get; }

        public CheckTicket(bool success, T? ticket)
        {
            Success = success;
            Ticket = ticket;
        }

        public bool TryGetValue([NotNullWhen(true)] out T? value)
        {
            return Success.Configure(value = Success ? Ticket.ThrowIfNull() : default);
        }
    }
}