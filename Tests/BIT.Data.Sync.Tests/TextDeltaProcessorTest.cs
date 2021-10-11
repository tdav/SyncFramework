using BIT.Data.Sync.Extensions;
using BIT.Data.Sync.Tests.Infrastructure;
using BIT.Data.Sync.TextImp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Tests
{
    public class TextDeltaProcessorTest : MultiServerBaseTest
    {
        TestClientFactory cf;
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
            cf = this.GetTestClientFactory();

        }


        [Test]
        public async Task ProcessDeltasAsync_Test()
        {

          
           
            List<SimpleDatabaseRecord> textRecords=new List<SimpleDatabaseRecord>();
            IDeltaProcessor deltaProcessor = new SimpleDatabaseDeltaProcessor(null, textRecords);


            List<IDelta> deltas = new List<IDelta>();
            MemoryDeltaStore memoryDeltaStore= new MemoryDeltaStore(deltas);
            List<SimpleDatabaseRecord> data = new List<SimpleDatabaseRecord>();
            SimpleDatabase db = new SimpleDatabase(memoryDeltaStore, "A", data);

            await db.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "Hello" });
            await db.Add(new SimpleDatabaseRecord() { Key = Guid.NewGuid(), Text = "World" });
            IEnumerable<IDelta> DeltasFromDeltaStore = await memoryDeltaStore.GetDeltasAsync(Guid.Empty, default);
            await deltaProcessor.ProcessDeltasAsync(DeltasFromDeltaStore, default);

            var test = textRecords.Count;
            
          
           

        }
    }
}