using BIT.Data.Sync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabase<T> where T : IRecord
    {
        public IDeltaProcessor DeltaProcessor { get; set; }
        public string Identity { get; set; }
        public IDeltaStore DeltaStore { get; set; }
        public SimpleDatabase(IDeltaProcessor deltaProcessor, string identity, IDeltaStore deltaStore)
        {
            DeltaProcessor = deltaProcessor;
            Identity = identity;
            DeltaStore = deltaStore;
        }
        List<T> Data;
        public async void Update(T Instance)
        {
            var ObjectToUpdate = Data.FirstOrDefault(x => x.Key == Instance.Key);
            if (ObjectToUpdate != null)
            {
                var Index = Data.IndexOf(ObjectToUpdate);
                Data[Index] = Instance;
                SimpleDatabaseModification item = new SimpleDatabaseModification(Operation.Update, Instance);
                await SaveDelta(item);
            }
          
        }

        private async Task SaveDelta(SimpleDatabaseModification item)
        {
            var Delta = DeltaStore.CreateDelta(Identity,item);
            await DeltaStore.SaveDeltasAsync(new List<IDelta>() { Delta }, default);
        }

        public void Delete(T Instance)
        {
            var ObjectToDelete=  Data.FirstOrDefault(x=>x.Key==Instance.Key);
            if(ObjectToDelete!=null)
            {
                Data.Remove(ObjectToDelete);
                modifications.Add(new SimpleDatabaseModification(Operation.Delete, Instance));
            }
           
        }
        public async Task Add(T Instance)
        {
            Data.Add(Instance);
           
            SimpleDatabaseModification item = new SimpleDatabaseModification(Operation.Add, Instance);
            await SaveDelta(item);
        }
    }
}
