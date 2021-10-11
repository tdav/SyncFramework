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

            StringBuilder currentText = new System.Text.StringBuilder();
           
            IDeltaProcessor deltaProcessor = new TextDeltaProcessor(null, currentText);
            var DeltaHello = deltaProcessor.CreateDelta("A", "Hello");
            var DeltaWorld = deltaProcessor.CreateDelta("A", "World");
            await deltaProcessor.ProcessDeltasAsync(new List<IDelta>{ DeltaHello, DeltaWorld },default);
            var Actual = currentText.ToString();

            var Expected = $"Hello{System.Environment.NewLine}World{System.Environment.NewLine}";
            Assert.AreEqual(Expected,Actual);

        }
    }
}