//using BIT.Data.Sync;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BIT.EfCore.Sync.Test
//{
//    public class EFDeltaStoreTest
//    {
//        [SetUp]
//        public void Setup()
//        {

//        }
//        public static byte[] CreateSpecialByteArray(int length)
//        {
//            var arr = new byte[length];
//            for (int i = 0; i < arr.Length; i++)
//            {
//                arr[i] = 0x20;
//            }
//            return arr;
//        }
//        [Test]
//        public void SaveDelta()
//        {
//            IDeltaStore eFDeltaStore = GetDeltaStore(nameof(SaveDelta));

//            var ByteArray = CreateSpecialByteArray(100);

//            Delta delta = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta);


//            var DeltaFromStore = eFDeltaStore.GetDeltas().FirstOrDefault();
//            Assert.AreEqual(DeltaFromStore.Operation.Length, delta.Operation.Length);
//            Assert.AreEqual(DeltaFromStore.Identity, delta.Identity);
//            Assert.AreEqual(DeltaFromStore.Epoch, delta.Epoch);
//        }

//        private static EFDeltaStore GetDeltaStore(string Name)
//        {
//            var builder = new DbContextOptionsBuilder<DeltaDbContext>();
//            builder.UseInMemoryDatabase(Name);
//            var options = builder.Options;
//            DeltaDbContext deltaDbContext = new DeltaDbContext(options);

//            //TODO fix
//            EFDeltaStore eFDeltaStore = new EFDeltaStore(deltaDbContext,null);
//            return eFDeltaStore;
//        }

//        [Test]
//        public void MarkDeltaAsProcessed()
//        {
//            IDeltaStore eFDeltaStore = GetDeltaStore(nameof(MarkDeltaAsProcessed));

//            var ByteArray = CreateSpecialByteArray(100);

//            Delta delta = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta);


//            var DeltaFromStore = eFDeltaStore.GetDeltas().FirstOrDefault();

//            eFDeltaStore.MarkDeltaAsProcessed(new IDelta[] { DeltaFromStore });

//            DeltaFromStore= eFDeltaStore.GetDeltas().FirstOrDefault();


//            Assert.IsTrue(DeltaFromStore.Processed);

//        }
//        [Test]
//        public void SaveMultipleDeltas()
//        {
//            IDeltaStore eFDeltaStore = GetDeltaStore(nameof(MarkDeltaAsProcessed));

//            var ByteArray = CreateSpecialByteArray(100);

//            Delta delta = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta);


//            Delta delta2 = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta2);

//            Delta delta3 = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta3);

//           var Deltas=    eFDeltaStore.GetDeltas();
//            foreach (var item in Deltas)
//            {
//                Debug.WriteLine(item.Oid);
//            }
//            Assert.AreEqual(eFDeltaStore.GetDeltaCount(), 3);

//        }

//        [Test]
//        public void CheckForProcessedDeltasTest()
//        {
//            IDeltaStore eFDeltaStore = GetDeltaStore(nameof(MarkDeltaAsProcessed));

//            var ByteArray = CreateSpecialByteArray(100);

//            Delta delta = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta);

//            var DeltaFromStore = eFDeltaStore.GetDeltas().FirstOrDefault();

//            eFDeltaStore.MarkDeltaAsProcessed(new IDelta[] { DeltaFromStore  });

//            DeltaFromStore = eFDeltaStore.GetDeltas().FirstOrDefault();

//            var Result = eFDeltaStore.CheckForProcessedDeltas(new GetDeltaParams[] { new GetDeltaParams() { DeltaKey = (int)DeltaFromStore.Oid, Identity = DeltaFromStore.Identity } });
//            Assert.AreEqual(DeltaFromStore.Oid, Result[0]);

//        }

//        [Test]
//        public void GetLastProcessedDeltaKey()
//        {
//            IDeltaStore eFDeltaStore = GetDeltaStore(nameof(MarkDeltaAsProcessed));

//            var ByteArray = CreateSpecialByteArray(100);

//            Delta delta = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta);

//            Delta delta2 = new Delta() { Date = new System.DateTime(1, 1, 1), Identity = "A", Processed = false, Operation = ByteArray };
//            eFDeltaStore.SaveDelta(delta2);

//            var DeltaFromStore = eFDeltaStore.GetDeltas();

//            eFDeltaStore.MarkDeltaAsProcessed(DeltaFromStore);

//            DeltaFromStore = eFDeltaStore.GetDeltas();



//            var Key=  eFDeltaStore.GetLastProcessedDeltaKey();

//            Assert.AreEqual(DeltaFromStore.Last().Oid, Key);

//        }
//    }
//}