using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
