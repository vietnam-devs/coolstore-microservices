using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace N8T.Core.Specification
{
    /// <summary>
    /// https://codereview.stackexchange.com/questions/166460/chaining-multiple-predicates
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Build<T>(string propertyName, string comparison, string value)
        {
            const string parameterName = "x";
            var parameter = Expression.Parameter(typeof(T), parameterName);
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var p = a.Parameters[0];

            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };

            Expression body = Expression.And(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            var p = a.Parameters[0];

            var visitor = new SubstExpressionVisitor { Subst = { [b.Parameters[0]] = p } };

            Expression body = Expression.Or(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            return comparison switch
            {
                "==" => MakeBinary(ExpressionType.Equal, left, value),
                "!=" => MakeBinary(ExpressionType.NotEqual, left, value),
                ">" => MakeBinary(ExpressionType.GreaterThan, left, value),
                ">=" => MakeBinary(ExpressionType.GreaterThanOrEqual, left, value),
                "<" => MakeBinary(ExpressionType.LessThan, left, value),
                "<=" => MakeBinary(ExpressionType.LessThanOrEqual, left, value),
                "Contains" or "StartsWith" or "EndsWith" => Expression.Call(MakeString(left), comparison,
                    Type.EmptyTypes, Expression.Constant(value, typeof(string))),
                "In" => MakeList(left, value.Split(',')),
                _ => throw new NotSupportedException($"Invalid comparison operator '{comparison}'."),
            };
        }

        private static Expression MakeList(Expression left, IEnumerable<string> codes)
        {
            var objValues = codes.Cast<object>().ToList();
            var type = typeof(List<object>);
            var methodInfo = type.GetMethod("Contains", new[] { typeof(object) });
            var list = Expression.Constant(objValues);
            var body = Expression.Call(list, methodInfo, left);
            return body;
        }

        private static Expression MakeString(Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }

            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }

        private class SubstExpressionVisitor : ExpressionVisitor
        {
            public readonly Dictionary<Expression, Expression> Subst = new();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return Subst.TryGetValue(node, out var newValue) ? newValue : node;
            }
        }
    }
}
