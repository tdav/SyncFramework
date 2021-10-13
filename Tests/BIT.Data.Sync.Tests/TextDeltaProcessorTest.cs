using BIT.Data.Sync.Extensions;
using BIT.Data.Sync.Tests.Infrastructure;
using BIT.Data.Sync.TextImp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Tests
{
    public class TextDeltaProcessorTest 
    {
       
        [SetUp()]
        public  void Setup()
        {
        
            

        }
        [Test]
        public async Task SyncMultipleClients_Add_Test()
        {

            MemoryDeltaStore MasterDeltaStore = new MemoryDeltaStore();

            SimpleDatabase Master = new SimpleDatabase(MasterDeltaStore, "Master");
            SimpleDatabaseDeltaProcessor Master_DeltaProcessor = new SimpleDatabaseDeltaProcessor(null, Master.Data);
            await Master.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hello" });
            await Master.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "World" });

            //Creating Database A
            MemoryDeltaStore A_DeltaStore = new MemoryDeltaStore();
            SimpleDatabase A_Database = new SimpleDatabase(A_DeltaStore, "A");
            SimpleDatabaseDeltaProcessor A_DeltaProcessor = new SimpleDatabaseDeltaProcessor(null,A_Database.Data);

            await A_Database.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hola" });
            await A_Database.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Mundo" });

            //Creating Database A
            MemoryDeltaStore B_DeltaStore = new MemoryDeltaStore();
            SimpleDatabase B_Database = new SimpleDatabase(B_DeltaStore, "B");
            SimpleDatabaseDeltaProcessor B_DeltaProcessor= new SimpleDatabaseDeltaProcessor(null, B_Database.Data);

            await B_Database.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Privet" });
            await B_Database.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "mir" });


            Task<IEnumerable<IDelta>> DeltasFromDatabaseA = A_Database.DeltaStore.GetDeltasAsync(Guid.Empty, default);
            Task<IEnumerable<IDelta>> DeltasFromDatabaseB = B_Database.DeltaStore.GetDeltasAsync(Guid.Empty, default);
            Task<IEnumerable<IDelta>> DeltasFromMaster = Master.DeltaStore.GetDeltasAsync(Guid.Empty, default);
            
            
            await Master_DeltaProcessor.ProcessDeltasAsync(await DeltasFromDatabaseA, default);
            await Master_DeltaProcessor.ProcessDeltasAsync(await DeltasFromDatabaseB, default);

            await A_DeltaProcessor.ProcessDeltasAsync(await DeltasFromDatabaseB, default);
            await A_DeltaProcessor.ProcessDeltasAsync(await DeltasFromMaster, default);


            await B_DeltaProcessor.ProcessDeltasAsync(await DeltasFromDatabaseA, default);
            await B_DeltaProcessor.ProcessDeltasAsync(await DeltasFromMaster, default);


            Debug.WriteLine("Data in master");
            Master.Data.ForEach(r => Debug.WriteLine(r.ToString()));

            Debug.WriteLine("Data in A_Database");
            A_Database.Data.ForEach(r => Debug.WriteLine(r.ToString()));

            Debug.WriteLine("Data in B_Database");
            B_Database.Data.ForEach(r => Debug.WriteLine(r.ToString()));
        }
        [Test]
        public async Task SyncMultipleClients_Add_And_Remove_Test()
        {

            MemoryDeltaStore MasterDeltaStore = new MemoryDeltaStore();

            SimpleDatabase Master = new SimpleDatabase(MasterDeltaStore, "Master");
            SimpleDatabaseDeltaProcessor Master_DeltaProcessor = new SimpleDatabaseDeltaProcessor(null, Master.Data);
            SimpleDatabaseRecord Hello = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hello" };
            SimpleDatabaseRecord World = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "World" };
            
            await Master.Add(Hello);
            await Master.Add(World);

            //Creating Database A
            MemoryDeltaStore A_DeltaStore = new MemoryDeltaStore();
            SimpleDatabase A_Database = new SimpleDatabase(A_DeltaStore, "A");
            SimpleDatabaseDeltaProcessor A_DeltaProcessor = new SimpleDatabaseDeltaProcessor(null, A_Database.Data);

            SimpleDatabaseRecord Hola = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hola" };
            SimpleDatabaseRecord Mundo = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Mundo" };
            
            await A_Database.Add(Hola);
            await A_Database.Add(Mundo);

            //Creating Database A
            MemoryDeltaStore B_DeltaStore = new MemoryDeltaStore();
            SimpleDatabase B_Database = new SimpleDatabase(B_DeltaStore, "B");
            SimpleDatabaseDeltaProcessor B_DeltaProcessor = new SimpleDatabaseDeltaProcessor(null, B_Database.Data);

            SimpleDatabaseRecord Privet = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Privet" };
            SimpleDatabaseRecord Mir = new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "mir" };
            await B_Database.Add(Privet);
            await B_Database.Add(Mir);


            var DeltasFromDatabaseA = await A_Database.DeltaStore.GetDeltasAsync(Guid.Empty, default);
            var DeltasFromDatabaseB = await B_Database.DeltaStore.GetDeltasAsync(Guid.Empty, default);
            var DeltasFromMaster = await Master.DeltaStore.GetDeltasAsync(Guid.Empty, default);

           
            await Master_DeltaProcessor.ProcessDeltasAsync(DeltasFromDatabaseA, default);
            await Master.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromDatabaseA.Max(d => d.Index), default);
            await Master_DeltaProcessor.ProcessDeltasAsync(DeltasFromDatabaseB, default);
            await Master.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromDatabaseB.Max(d => d.Index), default);


            await A_DeltaProcessor.ProcessDeltasAsync(DeltasFromDatabaseB, default);
            await A_Database.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromDatabaseB.Max(d => d.Index), default);
            await A_DeltaProcessor.ProcessDeltasAsync(DeltasFromMaster, default);
            await A_Database.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromMaster.Max(d => d.Index), default);


            await B_DeltaProcessor.ProcessDeltasAsync(DeltasFromDatabaseA, default);
            await B_Database.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromDatabaseA.Max(d => d.Index), default);
            await B_DeltaProcessor.ProcessDeltasAsync(DeltasFromMaster, default);
            await B_Database.DeltaStore.SetLastProcessedDeltaAsync(DeltasFromMaster.Max(d => d.Index), default);


            Debug.WriteLine("Data in master");
            Master.Data.ForEach(r => Debug.WriteLine(r.ToString()));
            Debug.WriteLine("Data in master Last Processed Delta Index:" + await Master.DeltaStore.GetLastProcessedDeltaAsync(default));

            Debug.WriteLine("Data in A_Database");
            A_Database.Data.ForEach(r => Debug.WriteLine(r.ToString()));
            Guid A_LastIndexProccesded = await A_Database.DeltaStore.GetLastProcessedDeltaAsync(default);
            Debug.WriteLine("Data in A_Database Last Processed Delta Index:" + A_LastIndexProccesded);

            Debug.WriteLine("Data in B_Database");
            B_Database.Data.ForEach(r => Debug.WriteLine(r.ToString()));
            Guid B_LastIndexProccesded = await B_Database.DeltaStore.GetLastProcessedDeltaAsync(default);
            Debug.WriteLine("Data in B_Database Last Processed Delta Index:" + B_LastIndexProccesded);


            //Lets delete
            Master.Delete(Hola);

            
            await A_DeltaProcessor.ProcessDeltasAsync(await Master.DeltaStore.GetDeltasAsync(A_LastIndexProccesded, default),default);
            await B_DeltaProcessor.ProcessDeltasAsync(await Master.DeltaStore.GetDeltasAsync(B_LastIndexProccesded, default), default);


            Debug.WriteLine("Data in master");
            Master.Data.OrderBy(x => x.Key).ToList().ForEach(r => Debug.WriteLine(r.ToString()));
            Debug.WriteLine("Data in A_Database");
            A_Database.Data.OrderBy(x=>x.Key).ToList().ForEach(r => Debug.WriteLine(r.ToString()));
            Debug.WriteLine("Data in B_Database");
            B_Database.Data.OrderBy(x=>x.Key).ToList().ForEach(r => Debug.WriteLine(r.ToString()));
        }
        [Test]
        public async Task ProcessDeltasAsync_Test()
        {

          
           
            List<SimpleDatabaseRecord> textRecords=new List<SimpleDatabaseRecord>();
            IDeltaProcessor deltaProcessor = new SimpleDatabaseDeltaProcessor(null, textRecords);


           
            MemoryDeltaStore memoryDeltaStore= new MemoryDeltaStore();

            SimpleDatabase db = new SimpleDatabase(memoryDeltaStore, "A");

            await db.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hello" });
            await db.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "World" });

            IEnumerable<IDelta> DeltasFromDeltaStore = await memoryDeltaStore.GetDeltasAsync(Guid.Empty, default);
            await deltaProcessor.ProcessDeltasAsync(DeltasFromDeltaStore, default);

            var test = textRecords.Count;
            
          
           

        }
    }
}