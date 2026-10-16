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
            return CheckTicket<T>.Create(success, ticket);
        }
    }

    public readonly struct CheckTicket<T>
    {
        public T? Ticket { get; }

        [MemberNotNullWhen(true, nameof(Ticket))]
        public bool Success { get; }

        public CheckTicket() { }

        private CheckTicket(bool success, T? ticket)
        {
            Success = success;
            Ticket = ticket;
        }

        public bool TryGetValue([NotNullWhen(true)] out T? value)
        {
            return Success.Configure(value = Ticket);
        }

        public static CheckTicket<T> Create(bool success, T? ticket = default)
        {
            if (success)
            {
                ArgumentNullException.ThrowIfNull(ticket);
            }
            return new CheckTicket<T>(success, ticket);
        }
    }
}