using BIT.Data.Sync;
using BIT.Data.Sync.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.EfCore.Sync.DeltaProcessors
{
    public class MemoryDeltaProcessor : DeltaProcessorBase
    {
        public MemoryDeltaProcessor(DeltaStoreSettings deltaStoreSettings) : base(deltaStoreSettings)
        {

        }

     
        public override Task ProcessDeltasAsync(IEnumerable<IDelta> Detlas, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}