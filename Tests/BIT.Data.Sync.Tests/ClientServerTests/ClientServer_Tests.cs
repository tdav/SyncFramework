using BIT.Data.Sync.Tests.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Tests.ClientServerTests
{
    public class ClientServer_Tests : MultiServerBaseTest
    {
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
        }
        [Test]
        public async Task Push_Test()
        {
            var HttpClient=  TestServerClientFactory.CreateClient("");
        }
    }
}
