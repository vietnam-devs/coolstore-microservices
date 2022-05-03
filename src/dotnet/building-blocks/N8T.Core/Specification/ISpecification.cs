using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace N8T.Core.Specification
{
    public interface IRootSpecification
    {
    }

    /// <summary>
    /// https://stackoverflow.com/questions/63082758/ef-core-specification-pattern-add-all-column-for-sorting-data-with-custom-specif
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> : IRootSpecification
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }

        bool IsSatisfiedBy(T obj);
    }

    public interface IGridSpecification<T> : IRootSpecification
    {
        List<Expression<Func<T, bool>>> Criterias { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; set; }
    }
}
