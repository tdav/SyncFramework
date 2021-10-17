using BIT.Data.Sync;
using BIT.Data.Sync.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Tests.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase
    {


        private readonly ILogger<SyncController> _logger;
        private readonly ISyncServer _SyncServer;
        protected string GetHeader(string HeaderName)
        {
            return HttpContext.Request.Headers[HeaderName];
        }


        public SyncController(ILogger<SyncController> logger, ISyncServer SyncServer)
        {
            _logger = logger;
            _SyncServer = SyncServer;
        }
        [HttpPost(nameof(Push))]
        public virtual async Task Push()
        { 

           
            var stream = new StreamReader(this.Request.Body);
            var body = await stream.ReadToEndAsync();
           
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(body)))
            {
              
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<Delta>));
                List<Delta> Deltas = (List<Delta>)deserializer.ReadObject(ms);
                string NodeId = GetHeader("NodeId");
                await _SyncServer.SaveDeltasAsync(NodeId, Deltas, new CancellationToken());

            }

           
            
        }
        [HttpGet("Fetch")]
        public async Task<string> Fetch(Guid startindex, string identity)
        {
            string NodeId = GetHeader("NodeId");
            IEnumerable<IDelta> enumerable = await _SyncServer.GetDeltasAsync(NodeId, startindex, identity, new CancellationToken());
            List<Delta> toserialzie = new List<Delta>();
            foreach (IDelta delta in enumerable)
            {
                toserialzie.Add(new Delta(delta));
            }
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<Delta>));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, toserialzie);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string jsonDeltas = sr.ReadToEnd();
            return jsonDeltas;

        }

    }
}
