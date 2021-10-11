using BIT.Data.Sync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabase 
    {
        public IDeltaProcessor DeltaProcessor { get; set; }
        public string Identity { get; set; }
        public IDeltaStore DeltaStore { get; set; }
        public SimpleDatabase(IDeltaStore deltaStore, string identity,  List<SimpleDatabaseRecord> Data)
        {
          
            Identity = identity;
            DeltaStore = deltaStore;
            this.Data= Data;
        }
        List<SimpleDatabaseRecord> Data;
        public async void Update(SimpleDatabaseRecord Instance)
        {
            var ObjectToUpdate = Data.FirstOrDefault(x => x.Key == Instance.Key);
            if (ObjectToUpdate != null)
            {
                var Index = Data.IndexOf(ObjectToUpdate);
                Data[Index] = Instance;
                SimpleDatabaseModification item = new SimpleDatabaseModification(OperationType.Update, Instance);
                await SaveDelta(item);
            }
          
        }

        private async Task SaveDelta(SimpleDatabaseModification item)
        {
            var Delta = DeltaStore.CreateDelta(Identity,item);
            await DeltaStore.SaveDeltasAsync(new List<IDelta>() { Delta }, default);
        }

        public void Delete(SimpleDatabaseRecord Instance)
        {
            var ObjectToDelete=  Data.FirstOrDefault(x=>x.Key==Instance.Key);
            if(ObjectToDelete!=null)
            {
                Data.Remove(ObjectToDelete);
               
            }
           
        }
        public async Task Add(SimpleDatabaseRecord Instance)
        {
            Data.Add(Instance);
           
            SimpleDatabaseModification item = new SimpleDatabaseModification(OperationType.Add, Instance);
            await SaveDelta(item);
        }
    }
}
