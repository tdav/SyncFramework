using BIT.Data.Sync.Tests.Infrastructure;
using BIT.Data.Sync.TextImp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using BIT.Data.Sync.Extensions;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text;

namespace BIT.Data.Sync.Tests
{
        public class IntegrationTest : MultiServerBaseTest
    {
        TestClientFactory cf;
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
            cf = this.GetTestClientFactory();

        }
 

        [Test]
        public async Task SyncTwoTextDeltaProcessors_Test()
        {
            IDeltaStore memoryDeltaStore = new TextImp.MemoryDeltaStore(new List<IDelta>());

            var  DeltaHello=  memoryDeltaStore.CreateDelta("A", "Hello");
            var DeltaWorld = memoryDeltaStore.CreateDelta("B", "Hello");

            await memoryDeltaStore.SaveDeltasAsync(new List<IDelta>(){ DeltaHello, DeltaWorld },default);

            var DeltasFromStore = await memoryDeltaStore.GetDeltasAsync(Guid.Empty, default);

            StringBuilder StringBuilderA = new StringBuilder();
            IDeltaProcessor DeltaProcessorA = new SimpleDatabaseDeltaProcessor(null, StringBuilderA);


            StringBuilder StringBuilderB = new System.Text.StringBuilder();
            IDeltaProcessor DeltaProcessorB = new SimpleDatabaseDeltaProcessor(null, StringBuilderB);

            await DeltaProcessorA.ProcessDeltasAsync(DeltasFromStore, default);
            await DeltaProcessorB.ProcessDeltasAsync(DeltasFromStore, default);


            string expected = StringBuilderA.ToString();
            string actual = StringBuilderB.ToString();
            Assert.AreEqual(expected, actual);

        }
        
    }
}