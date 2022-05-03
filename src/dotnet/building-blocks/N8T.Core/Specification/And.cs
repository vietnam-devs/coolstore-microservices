using System;
using System.Linq.Expressions;

namespace N8T.Core.Specification
{
    public class And<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public And(
            ISpecification<T> left,
            ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        // AndSpecification
        public override Expression<Func<T, bool>> Criteria
        {
            get
            {
                var objParam = Expression.Parameter(typeof(T), "obj");

                var newExpr = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        Expression.Invoke(_left.Criteria, objParam),
                        Expression.Invoke(_right.Criteria, objParam)
                    ),
                    objParam
                );

                return newExpr;
            }
        }
    }
}
