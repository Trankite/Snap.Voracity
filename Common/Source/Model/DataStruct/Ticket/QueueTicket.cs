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
        public T Ticket { get; }

        public int Index { get; }

        public QueueTicket(int index, T ticket)
        {
            Index = index;
            Ticket = ticket;
        }
    }
}