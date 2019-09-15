using System.Data;

namespace CloudNativeKit.Infrastructure.Data.Dapper
{
    public interface IDynamicSqlConnectionFactory
    {
        IDbConnection GetOpenConnection(string dbConnString);
    }
}
