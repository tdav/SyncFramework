using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync
{
    public interface IDeltaStore
    {
        string Identity { get; }
        void SetIdentity(string Identity);
        Task SaveDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken);
        Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, string identity, CancellationToken cancellationToken);
        Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, CancellationToken cancellationToken);
        Task<IEnumerable<IDelta>> GetDeltasToSendAsync(Guid startindex, CancellationToken cancellationToken);
        Task<int> GetDeltaCountAsync(Guid startindex, CancellationToken cancellationToken);
        Task<Guid> GetLastProcessedDeltaAsync(CancellationToken cancellationToken);
        Task SetLastProcessedDeltaAsync(Guid Index, CancellationToken cancellationToken);
        Task<Guid> GetLastPushedDeltaAsync(CancellationToken cancellationToken);
        Task SetLastPushedDeltaAsync(Guid Index, CancellationToken cancellationToken);
        Task PurgeDeltasAsync(CancellationToken cancellationToken);

    }
}