using Microsoft.Extensions.Options;

namespace CloudNativeKit.Infrastructure.Data.EfCore.SqlServer
{
    public class SqlServerDbConnStringFactory : IDbConnStringFactory
    {
        public SqlServerDbOptions DbOptions { get; private set; }

        public SqlServerDbConnStringFactory()
        {
        }

        public SqlServerDbConnStringFactory(IOptions<SqlServerDbOptions> options)
        {
            DbOptions = options.Value;
        }

        public IDbConnStringFactory SetBasePath(string basePath)
        {
            var config = ConfigurationHelper.GetConfiguration(basePath);
            var dbConn = config.GetSection("ConnectionStrings:MainDb")?.Value;
            DbOptions = new SqlServerDbOptions { MainDb = dbConn };
            return this;
        }

        public string Create()
        {
            return DbOptions.MainDb;
        }
    }
}
