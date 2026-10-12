namespace Common.Source.Factory.Serialize.Interface
{
    public interface IContentSerializer<TDestination>
    {
        TDestination Serialize();
    }

    public interface IContentSerializer<TDestination, TSource> : IContentSerializer<TDestination>
    {
        TSource Deserialize(TDestination source);
    }
}