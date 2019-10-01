using System.Threading;
using System.Threading.Tasks;

namespace CloudNativeKit.Infrastructure.Bus.Messaging
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
