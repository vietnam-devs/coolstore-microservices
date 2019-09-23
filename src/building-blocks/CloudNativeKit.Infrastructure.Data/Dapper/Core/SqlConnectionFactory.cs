using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Core
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly IOptions<DapperDbOptions> _dapperOptions;
        private IDbConnection _connection;

        public SqlConnectionFactory(IOptions<DapperDbOptions> dapperOptions)
        {
            _dapperOptions = dapperOptions;
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_dapperOptions.Value?.MainDb);
                _connection.Open();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Dispose();
            }
        }
    }
}
