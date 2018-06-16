using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VND.Fw.Domain
{
    public interface IRepositoryFactory
    {
        IRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : IEntity;
    }

    public interface IRepositoryAsync<TEntity> where TEntity : IEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
    }

    public interface IQueryRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> Queryable();
    }
}
