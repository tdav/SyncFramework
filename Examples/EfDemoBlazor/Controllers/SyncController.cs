using BIT.Data.Sync;
using BIT.Data.Sync.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EfDemoBlazor.Controllers
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
            return this.HttpContext.Request.Headers[HeaderName];
        }

        
    
        public SyncController(ILogger<SyncController> logger, ISyncServer SyncServer)
        {
           
            _logger = logger;
            _SyncServer = SyncServer;
        }
        [HttpPost(nameof(Push))]
        public virtual async Task Push([FromBody] List<Delta> deltas)
        {
            var Identity= deltas.FirstOrDefault().Identity;
            var DeltaCount = deltas.Count;
            string DeltaStoreName = GetHeader("DeltaStoreName");
            string DeltaProcessorName = GetHeader("DeltaProcessorName");
            var Message = string.Format("Push Delta Store:{0} Delta Processor: {1} Deltas Received: {2} Identity: {3}", DeltaStoreName, DeltaProcessorName, Identity, DeltaCount);
            _logger.LogInformation(Message);
         

            await this._SyncServer.SaveDeltasAsync(deltas, DeltaStoreName,new CancellationToken());
           
        }
        [HttpGet("Fetch")]
        public async Task<IEnumerable<IDelta>> Fetch(Guid startindex, string identity)
        {
            string name = GetHeader("DeltaStoreName");
            _logger.LogInformation("Fetch Start Index:{0} Identity:{1}", startindex, identity);
            IEnumerable<IDelta> enumerable = await this._SyncServer.GetDeltasAsync(name, startindex, identity,new CancellationToken());
            var Message = string.Format("Fetch invoked Identity:{0} Delta Store {1} Start Index:{2} Deltas returned:{3}  ", identity, name, startindex, enumerable.Count());
            _logger.LogInformation(Message);
            
            

            return enumerable;

        }
       

    }
}
