using System.Linq;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;
using Microsoft.Extensions.Options;

namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper.Internal
{
    internal class SqlDbConnStringFactory : IDbConnStringFactory
    {
        public DapperDbOptions DbOptions { get; }

        public SqlDbConnStringFactory(IOptions<DapperDbOptions> options)
        {
            DbOptions = options.Value;
        }

        public string Create()
        {
            var connPattern = DbOptions.ConnString;
            var connConfigs = DbOptions.Host?.Split(':');
            var host = connConfigs?.First();
            var port = connConfigs?.Except(new[] { host }).First();

            return string.Format(
                connPattern,
                host, port,
                DbOptions.UserName,
                DbOptions.Password,
                DbOptions.Database);
        }
    }
}
