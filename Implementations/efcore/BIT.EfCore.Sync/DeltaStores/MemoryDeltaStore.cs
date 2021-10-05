using BIT.Data.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BIT.EfCore.Sync.DeltaStores
{
    public class MemoryDeltaStore : DeltaStoreBase
    {
        protected MemoryDeltaStore()
        {

        }
        List<IDelta> Deltas = new List<IDelta>();
      

        public override Task SaveDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Deltas.AddRange(deltas);
            return Task.CompletedTask;
        }

        public override Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, string identity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Deltas.Where(d => d.Index.CompareTo(startindex) > 0 && string.Compare(d.Identity, identity, StringComparison.Ordinal) != 0));
        }

        public override Task<IEnumerable<IDelta>> GetDeltasToSendAsync(Guid startindex, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task<Guid> GetLastProcessedDeltaAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task SetLastProcessedDeltaAsync(Guid Index, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task<Guid> GetLastPushedDeltaAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task SetLastPushedDeltaAsync(Guid Index, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public override Task<int> GetDeltaCountAsync(Guid startindex, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task PurgeDeltasAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}