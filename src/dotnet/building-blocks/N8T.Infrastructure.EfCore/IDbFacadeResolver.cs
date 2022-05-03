using Microsoft.EntityFrameworkCore.Infrastructure;

namespace N8T.Infrastructure.EfCore
{
    public interface IDbFacadeResolver
    {
        DatabaseFacade Database { get; }
    }
}