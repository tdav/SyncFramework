
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Server
{


    public interface ISyncServer
    {
        Task SaveDeltasAsync(IEnumerable<IDelta> deltas, string name, CancellationToken cancellationToken);
        Task<IEnumerable<IDelta>> GetDeltasAsync(string name, Guid startindex, string identity, CancellationToken cancellationToken);
        Task ProcessDeltasAsync(IEnumerable<IDelta> deltas, string Name, CancellationToken cancellationToken);
       

    }
}
