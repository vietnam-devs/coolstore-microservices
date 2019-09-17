using Microsoft.Extensions.Options;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Core
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
            return DbOptions.MainDb;
        }
    }
}
