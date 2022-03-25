using System;
using System.Linq.Expressions;

namespace N8T.Core.Specification;

public class NoOpSpec<TEntity> : SpecificationBase<TEntity>
{
    public override Expression<Func<TEntity, bool>> Criteria => p => true;
}
