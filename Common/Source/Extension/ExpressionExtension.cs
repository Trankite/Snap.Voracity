using System.Diagnostics;
using System.Linq.Expressions;

namespace Common.Source.Extension
{
    public static class ExpressionExtension
    {
        [DebuggerStepThrough]
        public static MemberExpression? MemberExpression(this Expression expression)
        {
            if (expression is not MemberExpression memberExpression)
            {
                if (expression is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
                {
                    return unaryExpression.Operand.MemberExpression();
                }
                return null;
            }
            return memberExpression;
        }

        [DebuggerStepThrough]
        public static string FullName<T>(this Expression<Func<T>> expression)
        {
            return GetFullName(expression.Body as MemberExpression);
        }

        [DebuggerStepThrough]
        public static string FullName<T1, T2>(this Expression<Func<T1, T2>> expression)
        {
            return GetFullName(expression.Body as MemberExpression);
        }

        private static string GetFullName(MemberExpression? memberExpression)
        {
            Stack<string> pathStack = new();
            while (memberExpression.IsNotNull())
            {
                pathStack.Push(memberExpression.Member.Name);
                if (memberExpression.Expression is not MemberExpression ParentExpression) break;
                memberExpression = ParentExpression;
            }
            return string.Join('.', pathStack);
        }
    }
}