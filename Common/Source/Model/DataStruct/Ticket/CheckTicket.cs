namespace Common.Source.Model.DataStruct.Ticket
{
    public static class CheckTicket
    {
        public static CheckTicket<T> Create<T>(T ticket)
        {
            return new CheckTicket<T>(true, ticket);
        }

        public static CheckTicket<T> Create<T>(bool success, T? ticket)
        {
            return new CheckTicket<T>(success, ticket);
        }
    }

    public readonly struct CheckTicket<T>
    {
        public readonly T? Ticket;

        public readonly bool Success;

        public CheckTicket() { }

        public CheckTicket(bool success, T? ticket)
        {
            Success = success;
            Ticket = ticket;
        }
    }
}