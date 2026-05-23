using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Extension
{
    public static class ObjectExtension
    {
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>(this T? value)
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
        public static T NotNull<T>(this T? value, T defaultValue)
        {
            return value ?? defaultValue;
        }

        [DebuggerStepThrough]
        public static T NotNull<T>(this T? value, Func<T> getter)
        {
            return value ?? getter();
        }

        [DebuggerStepThrough]
        public static T Middle<T>([NotNullWhen(true)] this T value, T minimum, T maximum) where T : IComparable<T>
        {
            return value.CompareTo(minimum) < 0 ? minimum : value.CompareTo(maximum) < 0 ? value : maximum;
        }

        [DebuggerStepThrough]
        public static bool IsDefault<T>(this T? value)
        {
            return EqualityComparer<T>.Default.Equals(value, default);
        }

        [DebuggerStepThrough]
        public static bool IsNotDefault<T>(this T? value) => !value.IsDefault();

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
        public static TResult Captured<TResult, TNone>(this TNone _, TResult result) where TNone : allows ref struct where TResult : allows ref struct
        {
            return result;
        }

        [DebuggerStepThrough]
        public static bool OutTemp<TSelf>(this TSelf value, out TSelf self) where TSelf : allows ref struct
        {
            return true.Configure(self = value);
        }

        [DebuggerStepThrough]
        public static TSelf OutSelf<TSelf>(this TSelf value, out TSelf self) where TSelf : allows ref struct
        {
            return self = value;
        }
    }
}