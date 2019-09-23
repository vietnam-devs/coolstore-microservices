using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Data.EfCore.Core.Command
{
    public static class EfUnitOfWorkExtensions
    {
        public static int? GetCommandTimeout(this IEfUnitOfWork uow, DbContext context)
        {
            return context.Database.GetCommandTimeout();
        }

        public static IUnitOfWork SetCommandTimeout(this IEfUnitOfWork uow, DbContext context, int? value)
        {
            context.Database.SetCommandTimeout(value);
            return uow;
        }

        public static int ExecuteSqlCommand(this IEfUnitOfWork uow, DbContext context, string sql,
            params object[] parameters)
        {
            return context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public static async Task<int> ExecuteSqlCommandAsync(this IEfUnitOfWork uow, DbContext context, string sql,
            params object[] parameters)
        {
            return await context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public static async Task<int> ExecuteSqlCommandAsync(this IEfUnitOfWork uow, DbContext context, string sql,
            CancellationToken cancellationToken, params object[] parameters)
        {
            return await context.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
        }
    }
}
