using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Extension
{
    public static class ObjectExtension
    {
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>([NotNull] this T? value)
        {
            return value ?? throw new NullReferenceException();
        }

        [DebuggerStepThrough]
        public static bool IsNull<T>([NotNullWhen(false)] this T? value)
        {
            return value is null;
        }

        [DebuggerStepThrough]
        public static bool IsNotNull<T>([NotNullWhen(true)] this T? value)
        {
            return value is not null;
        }

        [DebuggerStepThrough]
        public static T NotNull<T>(this T? value) where T : new()
        {
            return value ?? new T();
        }

        [DebuggerStepThrough]
        public static T NotNull<T>(this T? value) where T : struct
        {
            return value ?? default;
        }

        [DebuggerStepThrough]
        public static T NotNull<T>(this T? value, T defaultValue)
        {
            return value ?? defaultValue;
        }

        [DebuggerStepThrough]
        public static T NotNull<T>(this T? value, Func<T> defaultValue)
        {
            return value ?? defaultValue.Invoke();
        }

        [DebuggerStepThrough]
        public static bool IsEquals<T>(this T? value, T? other)
        {
            return EqualityComparer<T>.Default.Equals(value, other);
        }

        [DebuggerStepThrough]
        public static bool IsNotEquals<T>(this T? value, T? other)
        {
            return !IsEquals(value, other);
        }

        [DebuggerStepThrough]
        public static bool IsEquals<T>(this T? value, T? other, IEqualityComparer<T>? comparer)
        {
            return (comparer ?? EqualityComparer<T>.Default).Equals(value, other);
        }

        [DebuggerStepThrough]
        public static bool IsNotEquals<T>(this T? value, T? other, IEqualityComparer<T>? comparer)
        {
            return !IsEquals(value, other, comparer);
        }

        [DebuggerStepThrough]
        public static void TryDispose<T>(this T? value) where T : IDisposable
        {
            try { value?.Dispose(); } catch { }
        }

        [DebuggerStepThrough]
        public static T Configure<T>(this T value, Action action) where T : allows ref struct
        {
            action.Invoke();
            return value;
        }

        [DebuggerStepThrough]
        public static T Configure<T>(this T value, Action<T> action) where T : allows ref struct
        {
            action.Invoke(value);
            return value;
        }

        [DebuggerStepThrough]
        public static TSelf Configure<TSelf, TNone>(this TSelf value, TNone _) where TSelf : allows ref struct where TNone : allows ref struct
        {
            return value;
        }

        [DebuggerStepThrough]
        public static TResult Captured<TNone, TResult>(this TNone _, TResult result) where TNone : allows ref struct where TResult : allows ref struct
        {
            return result;
        }

        [DebuggerStepThrough]
        public static TResult Captured<TSelf, TResult>(this TSelf value, Func<TSelf, TResult> action) where TSelf : allows ref struct where TResult : allows ref struct
        {
            return action.Invoke(value);
        }

        [DebuggerStepThrough]
        public static bool IsExchanged<T>(this T source, ref T destination)
        {
            return destination.IsNotEquals(source).Configure(destination = source);
        }

        [DebuggerStepThrough]
        public static T OutSelf<T>(this T value, out T self) where T : allows ref struct
        {
            return self = value;
        }
    }
}