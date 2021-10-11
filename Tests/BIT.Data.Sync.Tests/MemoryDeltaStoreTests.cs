using BIT.Data.Sync.Tests.Infrastructure;
using BIT.Data.Sync.TextImp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using BIT.Data.Sync.Extensions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace BIT.Data.Sync.Tests
{
        public class MemoryDeltaStoreTests : MultiServerBaseTest
    {
        TestClientFactory cf;
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
            cf = this.GetTestClientFactory();

        }
 

        [Test]
        public async Task SaveDeltasAsync_Test()
        {
            IDeltaStore memoryDeltaStore = new TextImp.MemoryDeltaStore(new List<IDelta>());

            var  DeltaHello=  memoryDeltaStore.CreateDelta("A", "Hello");

            await memoryDeltaStore.SaveDeltasAsync(new List<IDelta>(){ DeltaHello },default);

            Assert.AreEqual(1, await memoryDeltaStore.GetDeltaCountAsync(Guid.Empty,default));

        }
        [Test]
        public async Task GetDeltasAsync_Test()
        {
            IDeltaStore memoryDeltaStore = new TextImp.MemoryDeltaStore(new List<IDelta>());

            var DeltaHello = memoryDeltaStore.CreateDelta("A", "Hello");
            var DeltaWorld = memoryDeltaStore.CreateDelta("A", "World");
            
            await memoryDeltaStore.SaveDeltasAsync(new List<IDelta>() { DeltaHello , DeltaWorld }, default);

            IEnumerable<IDelta> DeltasFromStore = await memoryDeltaStore.GetDeltasAsync(Guid.Empty, default);
           

            Assert.NotNull(DeltasFromStore.FirstOrDefault(d=>d.Index==DeltaHello.Index));
            Assert.NotNull(DeltasFromStore.FirstOrDefault(d => d.Index == DeltaWorld.Index));
        }
        [Test]
        public async Task PurgeDeltasAsync_Test()
        {
            MemoryDeltaStore memoryDeltaStore = new MemoryDeltaStore(new List<IDelta>());

            var DeltaHello = memoryDeltaStore.CreateDelta("A", "Hello");

            await memoryDeltaStore.SaveDeltasAsync(new List<IDelta>() { DeltaHello });

            await memoryDeltaStore.PurgeDeltasAsync();
            Assert.AreEqual(0, await memoryDeltaStore.GetDeltaCountAsync(Guid.Empty));

        }
    }
}