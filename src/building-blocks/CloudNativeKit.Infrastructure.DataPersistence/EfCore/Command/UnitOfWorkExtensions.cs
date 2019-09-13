using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore.Command
{
    public static class EfUnitOfWorkExtensions
    {
        public static int? GetCommandTimeout(this IUnitOfWorkAsync uow, DbContext context)
        {
            return context.Database.GetCommandTimeout();
        }

        public static IUnitOfWorkAsync SetCommandTimeout(this IUnitOfWorkAsync uow, DbContext context, int? value)
        {
            context.Database.SetCommandTimeout(value);
            return uow;
        }

        public static int ExecuteSqlCommand(this IUnitOfWorkAsync uow, DbContext context, string sql,
            params object[] parameters)
        {
            return context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public static async Task<int> ExecuteSqlCommandAsync(this IUnitOfWorkAsync uow, DbContext context, string sql,
            params object[] parameters)
        {
            return await context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public static async Task<int> ExecuteSqlCommandAsync(this IUnitOfWorkAsync uow, DbContext context, string sql,
            CancellationToken cancellationToken, params object[] parameters)
        {
            return await context.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
        }
    }
}
