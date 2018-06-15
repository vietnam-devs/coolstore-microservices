using Microsoft.EntityFrameworkCore;

namespace VND.Fw.Domain
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
