using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
  public static class DbContextExtensions
  {
    public static bool AllMigrationsApplied(this DbContext context)
    {
      System.Collections.Generic.IEnumerable<string> applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

      System.Collections.Generic.IEnumerable<string> total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

      return !total.Except(applied).Any();
    }
  }
}
