using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VND.Fw.Domain;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
  public static class OrderByExtension
  {
    public static IQueryable<TEntity> OrderByPropertyName<TEntity>(this IQueryable<TEntity> source, string propertyName, bool isDescending)
        where TEntity : IEntity
    {
      if (source == null)
      {
        throw new ArgumentException("source");
      }

      if (string.IsNullOrWhiteSpace(propertyName))
      {
        throw new ArgumentException("propertyName");
      }

      Type type = typeof(TEntity);
      ParameterExpression arg = Expression.Parameter(type, "x");
      PropertyInfo propertyInfo = type.GetProperty(propertyName);
      Expression expression = Expression.Property(arg, propertyInfo);
      type = propertyInfo.PropertyType;

      Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
      LambdaExpression lambda = Expression.Lambda(delegateType, expression, arg);

      string methodName = isDescending ? "OrderByDescending" : "OrderBy";
      object result = typeof(Queryable).GetMethods().Single(
          method => method.Name == methodName
                  && method.IsGenericMethodDefinition
                  && method.GetGenericArguments().Length == 2
                  && method.GetParameters().Length == 2)
          .MakeGenericMethod(typeof(TEntity), type)
          .Invoke(null, new object[] { source, lambda });

      return (IQueryable<TEntity>)result;
    }
  }
}
