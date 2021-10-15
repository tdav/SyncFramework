using BIT.Data.Sync.Options;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Server
{
    public class SyncServerBase : ISyncServerNode
    {
        IOptionsSnapshot<DeltaStoreSettings> _options;
        ReflectionService _deltaStoreFactory;
        public SyncServerBase(IOptionsSnapshot<DeltaStoreSettings> options, ReflectionService deltaStoreFactory)
        {
            this._options = options;
            this._deltaStoreFactory = deltaStoreFactory;
        }
        public async Task<IEnumerable<IDelta>> GetDeltasAsync(string name, Guid startindex, string identity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Options = this._options.Get(name);
            var deltaStore = this._deltaStoreFactory.CreateDeltaStore(Options);
            if (deltaStore != null)
            {
        
                return await deltaStore.GetDeltasFromOtherNodes(startindex, identity, cancellationToken).ConfigureAwait(false); 
            }

            IEnumerable<IDelta> result = new List<IDelta>();
            return result;
        }

        public Task ProcessDeltasAsync(string Name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Options = this._options.Get(Name);
            var Processor = this._deltaStoreFactory.CreateDeltaProcessor(Options);
            return Processor.ProcessDeltasAsync(deltas, cancellationToken);

        }

        public Task SaveDeltasAsync(string name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Options = this._options.Get(name);
            var deltaStore = this._deltaStoreFactory.CreateDeltaStore(Options);

            if (deltaStore != null)
            {
                return deltaStore.SaveDeltasAsync(deltas,cancellationToken);
            }
            return Task.CompletedTask;
        }
    }

}
