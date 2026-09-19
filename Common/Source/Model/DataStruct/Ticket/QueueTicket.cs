namespace Common.Source.Model.DataStruct.Ticket
{
    public static class QueueTicket
    {
        public static QueueTicket<T> Create<T>(int index, T ticket)
        {
            return new QueueTicket<T>(index, ticket);
        }
    }

    public readonly struct QueueTicket<T>
    {
        public readonly int Index;

        public readonly T Ticket;

        public QueueTicket(int index, T ticket)
        {
            Index = index;
            Ticket = ticket;
        }
    }
}