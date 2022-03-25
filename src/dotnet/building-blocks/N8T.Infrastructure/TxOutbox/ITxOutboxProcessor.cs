using System;
using System.Threading;
using System.Threading.Tasks;

namespace N8T.Infrastructure.TxOutbox
{
    public interface ITxOutboxProcessor
    {
        Task HandleAsync(Type integrationAssemblyType, CancellationToken cancellationToken = new());
    }
}
