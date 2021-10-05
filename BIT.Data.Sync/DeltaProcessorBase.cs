using BIT.Data.Sync.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync
{
    public abstract class DeltaProcessorBase : IDeltaProcessor
    {
        protected DeltaStoreSettings _deltaStoreSettings;
        public DeltaProcessorBase(DeltaStoreSettings deltaStoreSettings)
        {
            _deltaStoreSettings = deltaStoreSettings;
        }

        public abstract Task ProcessDeltasAsync(IEnumerable<IDelta> Detlas, CancellationToken cancellationToken);

    }
}