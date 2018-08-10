using Microsoft.EntityFrameworkCore;

namespace VND.Fw.Infrastructure.EfCore.Db
{
  public interface ICustomModelBuilder
  {
    void Build(ModelBuilder modelBuilder);
  }
}
