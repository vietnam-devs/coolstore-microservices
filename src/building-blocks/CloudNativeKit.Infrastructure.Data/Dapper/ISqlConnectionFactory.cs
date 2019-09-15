using System.Data;

namespace CloudNativeKit.Infrastructure.Data.Dapper
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
