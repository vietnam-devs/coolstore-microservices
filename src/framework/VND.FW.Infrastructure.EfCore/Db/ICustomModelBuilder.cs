using Microsoft.EntityFrameworkCore;

namespace VND.FW.Infrastructure.EfCore.Db
{
		public interface ICustomModelBuilder
		{
				void Build(ModelBuilder modelBuilder);
		}
}
