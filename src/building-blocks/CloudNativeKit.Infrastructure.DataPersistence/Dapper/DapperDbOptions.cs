namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper
{
    public class DapperDbOptions
    {
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string ConnString { get; set; } = "Server=tcp:{0},{1};Database={2};User Id={3};Password={4};";
    }
}
