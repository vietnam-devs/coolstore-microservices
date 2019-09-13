using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;

namespace CloudNativeKit.Infrastructure.DataPersistence.InMemory
{
    internal class NoOpDbConnStringFactory : IDbConnStringFactory
    {
        public string Create()
        {
            return string.Empty;
        }
    }
}
