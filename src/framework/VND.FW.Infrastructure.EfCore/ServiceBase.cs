using VND.Fw.Domain;

namespace VND.FW.Infrastructure.EfCore
{
    public abstract class QueryServiceBase : IService
    {
    }

    public abstract class CommandServiceBase : IService
    {
        protected readonly IUnitOfWorkAsync UnitOfWork;
        protected CommandServiceBase(IUnitOfWorkAsync uow)
        {
            UnitOfWork = uow;
        }
    }
}
