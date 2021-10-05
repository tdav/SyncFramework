using BIT.Data.Sync.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync
{
    public abstract class DeltaStoreBase : IDeltaStore
    {

        protected DeltaStoreBase()
        {

        }
        protected DeltaStoreSettings _deltaStoreSettings;

        public string Identity { get; private set; }

        public DeltaStoreBase(DeltaStoreSettings deltaStoreSettings)
        {
            this._deltaStoreSettings = deltaStoreSettings;
            Setup();
        }
        protected virtual void Setup()
        {

        }
        public abstract Task SaveDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken = default);

        public abstract Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, string identity, CancellationToken cancellationToken = default);
        public abstract Task<IEnumerable<IDelta>> GetDeltasToSendAsync(Guid startindex, CancellationToken cancellationToken = default);
        public abstract Task<Guid> GetLastProcessedDeltaAsync(CancellationToken cancellationToken = default);
        public abstract Task SetLastProcessedDeltaAsync(Guid Index, CancellationToken cancellationToken = default);

        public abstract Task<IEnumerable<IDelta>> GetDeltasAsync(Guid startindex, CancellationToken cancellationToken = default);

        public void SetIdentity(string Identity)
        {
            this.Identity = Identity;
        }

        public abstract Task<Guid> GetLastPushedDeltaAsync(CancellationToken cancellationToken = default);
        public abstract Task SetLastPushedDeltaAsync(Guid Index, CancellationToken cancellationToken = default);

        public abstract Task<int> GetDeltaCountAsync(Guid startindex, CancellationToken cancellationToken=default);
        public abstract Task PurgeDeltasAsync(CancellationToken cancellationToken);
    }
}