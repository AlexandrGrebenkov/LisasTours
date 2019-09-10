using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LisasTours.Helpers
{
    /// <summary>
    /// Расширения для выражений
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Соединить коллекцию выражений по "И"
        /// </summary>
        /// <param name="filters">Коллекция выражений</param>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <returns>Итоговое выражение</returns>
        public static Expression<Func<T, bool>> And<T>(this IEnumerable<Expression<Func<T, bool>>> filters)
        {
            return filters.Where(_ => _ != null).Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(
                _ => true, ExpressionHelper.And);
        }

        /// <summary>
        ///     Композиция двух логических выражений c объектом по условию "и".
        ///     Предназначено для конструирования критериев выборки в IEnumerable и IQueryable
        /// </summary>
        /// <param name="left">левая часть выражения</param>
        /// <param name="right">правая часть выражения</param>
        /// <returns>
        ///     Композицию <paramref name="left" /> и <paramref name="right" />
        /// </returns>
        /// <typeparam name="T">Тип объекта</typeparam>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            var invokedExpr = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
        }

        /// <summary>
        ///     Композиция двух логических выражений c объектом по условию "или".
        ///     Предназначено для конструирования критериев выборки в IEnumerable и IQueryable
        /// </summary>
        /// <param name="left">левая часть выражения</param>
        /// <param name="right">правая часть выражения</param>
        /// <returns>
        ///     Композицию <paramref name="left" /> и <paramref name="right" />
        /// </returns>
        /// <typeparam name="T">Тип объекта</typeparam>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            var invokedExpr = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left.Body, invokedExpr), left.Parameters);
        }

        /// <summary>
        ///     Отрицание логического выражения c объектом.
        ///     Предназначено для конструирования критериев выборки в IEnumerable и IQueryable
        /// </summary>
        /// <param name="expr">Логическое выражение</param>
        /// <returns>
        ///     Отрицание <paramref name="expr" />
        /// </returns>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     Если <paramref name="expr" /> == <c>null</c>
        /// </exception>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException("expr");
            }

            var invokedExpr = Expression.Invoke(expr, expr.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.Not(invokedExpr), expr.Parameters);
        }
    }
}
