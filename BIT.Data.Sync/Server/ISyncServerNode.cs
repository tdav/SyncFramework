
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Server
{


    public interface ISyncServerNode
    {
        Task SaveDeltasAsync(string DeltaStoreId, IEnumerable<IDelta> deltas, CancellationToken cancellationToken);
        Task<IEnumerable<IDelta>> GetDeltasAsync(string DeltaStoreId, Guid startindex, string identity, CancellationToken cancellationToken);
        Task ProcessDeltasAsync(string DeltaStoreId, IEnumerable<IDelta> deltas, CancellationToken cancellationToken);
       

    }
}
