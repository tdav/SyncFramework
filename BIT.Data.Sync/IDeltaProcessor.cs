using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync
{
    public interface IDeltaProcessor
    {
        Task ProcessDeltasAsync(IEnumerable<IDelta> Detlas, CancellationToken cancellationToken);
    }
}