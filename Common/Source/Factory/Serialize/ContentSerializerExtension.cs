using Common.Source.Extension;
using Common.Source.Factory.Serialize.Interface;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Factory.Serialize
{
    public static class ContentSerializerExtension
    {
        [DebuggerStepThrough]
        public static bool TrySerialize<TDestination>(this IContentSerializer<TDestination?> serializer, [NotNullWhen(true)] out TDestination? destination)
        {
            return serializer.Serialize().IsNotNull(out destination);
        }

        [DebuggerStepThrough]
        public static bool TryDeserialize<TDestination, TSource>(this IContentSerializer<TDestination, TSource?> serializer, TDestination destination, [NotNullWhen(true)] out TSource? source)
        {
            return serializer.Deserialize(destination).IsNotNull(out source);
        }
    }
}