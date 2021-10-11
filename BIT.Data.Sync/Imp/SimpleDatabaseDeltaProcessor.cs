using BIT.Data.Sync.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BIT.Data.Sync.Extensions;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabaseDeltaProcessor : DeltaProcessorBase
    {
        public SimpleDatabaseDeltaProcessor(DeltaStoreSettings deltaStoreSettings) : this(deltaStoreSettings,new StringBuilder())
        {
        }
        StringBuilder _CurrentText;
        public SimpleDatabaseDeltaProcessor(DeltaStoreSettings deltaStoreSettings,StringBuilder CurrentText) : base(deltaStoreSettings)
        {
            _CurrentText=CurrentText;
        }
        public override Task ProcessDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
           
            cancellationToken.ThrowIfCancellationRequested();
            foreach (IDelta delta in deltas)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var DeltaValue= this.GetDeltaOperations<string>(delta);
                this._CurrentText.AppendLine(DeltaValue);
            }
            return Task.CompletedTask;
            
        }
    }
}
