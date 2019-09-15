namespace CloudNativeKit.Infrastructure.Data.InMemory
{
    internal class NoOpDbConnStringFactory : IDbConnStringFactory
    {
        public string Create()
        {
            return string.Empty;
        }
    }
}
