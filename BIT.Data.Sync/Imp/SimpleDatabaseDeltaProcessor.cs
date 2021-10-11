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
    public class SimpleDatabaseDeltaProcessor :DeltaProcessorBase 
    {
        
        List<SimpleDatabaseRecord> _CurrentText;
        public SimpleDatabaseDeltaProcessor(DeltaStoreSettings deltaStoreSettings, List<SimpleDatabaseRecord> CurrentData) : base(deltaStoreSettings)
        {
            _CurrentText= CurrentData;
        }
        public override Task ProcessDeltasAsync(IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
           
            cancellationToken.ThrowIfCancellationRequested();
            foreach (IDelta delta in deltas)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var Modification= this.GetDeltaOperations<SimpleDatabaseModification>(delta);
                switch (Modification.Operation)
                {
                    case OperationType.Add:
                        this._CurrentText.Add(Modification.Record);
                        break;
                    case OperationType.Delete:
                        var ObjectToDelete=  this._CurrentText.FirstOrDefault(x=>x.Key==Modification.Record.Key);
                        this._CurrentText.Remove(ObjectToDelete);
                        break;
                    case OperationType.Update:
                        var ObjectToUpdate = this._CurrentText.FirstOrDefault(x => x.Key == Modification.Record.Key);
                        var Index= this._CurrentText.IndexOf(ObjectToUpdate);
                        this._CurrentText[Index] = Modification.Record;
                        break;
                }
              
                
            }
            return Task.CompletedTask;
            
        }
    }
}
