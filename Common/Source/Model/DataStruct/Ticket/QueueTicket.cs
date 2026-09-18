namespace Common.Source.Model.DataStruct.Ticket
{
    public static class QueueTicket
    {
        public static SerialTicket<T> Create<T>(int index, T ticket)
        {
            return new SerialTicket<T>(index, ticket);
        }
    }

    public readonly struct SerialTicket<T>
    {
        public readonly int Index;

        public readonly T Ticket;

        public SerialTicket(int index, T ticket)
        {
            Index = index;
            Ticket = ticket;
        }
    }
}