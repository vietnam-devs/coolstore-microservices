using System.Data;

namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper
{
    public interface IDynamicSqlConnectionFactory
    {
        IDbConnection GetOpenConnection(string dbConnString);
    }
}
