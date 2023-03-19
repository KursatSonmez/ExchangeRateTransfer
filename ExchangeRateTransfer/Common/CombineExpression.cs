using System;
using System.Collections.Generic;
using System.Linq.Expressions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ExchangeRateTransfer.DotNet")]
namespace ExchangeRateTransfer.Common
{
    internal static class CombineExpression
    {
        internal static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(
                func1.Body,
                new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        internal static Func<T, bool> AndAlso<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            return x => func1(x) && func2(x);
        }

        internal static Expression<Func<T1, T2>> AndAlso<T1, T2>(this Expression<Func<T1, T2>> func1, Expression<Func<T1, T2>> func2)
        {
            return Expression.Lambda<Func<T1, T2>>(Expression.AndAlso(
                func1.Body,
                new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        internal static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(
                func1.Body,
                new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        internal static Func<T, bool> OrElse<T>(this Func<T, bool> func1, Func<T, bool> func2)
        {
            return x => func1(x) || func2(x);
        }


        private sealed class ExpressionParameterReplacer : ExpressionVisitor
        {
            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                {
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
                }
            }


            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ParameterReplacements.TryGetValue(node, out ParameterExpression replacement))
                {
                    node = replacement;
                }
                return base.VisitParameter(node);
            }
        }
    }
}
