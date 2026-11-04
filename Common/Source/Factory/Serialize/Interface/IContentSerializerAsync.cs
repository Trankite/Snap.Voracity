namespace Common.Source.Factory.Serialize.Interface
{
    public interface IContentSerializerAsync<TDestination> : IContentSerializer<TDestination>
    {
        ValueTask<TDestination> SerializeAsync(CancellationToken cancellationToken = default);
    }

    public interface IContentSerializerAsync<TDestination, TSource> : IContentSerializer<TDestination, TSource>
    {
        ValueTask<TSource> DeserializeAsync(TDestination source, CancellationToken cancellationToken = default);
    }
}