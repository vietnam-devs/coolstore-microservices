namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db
{
    public interface IDbConnStringFactory
    {
        string Create();
    }
}
