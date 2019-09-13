using System.Data;

namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
