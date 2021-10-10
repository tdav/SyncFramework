﻿using BIT.Data.Sync;
using BIT.Data.Sync.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public virtual async Task Push([FromBody] List<Delta> deltas)
        {

            string DeltaStoreName = GetHeader("DeltaStoreName");
            string DeltaProcessorName = GetHeader("DeltaProcessorName");
            await _SyncServer.SaveDeltasAsync(deltas, DeltaStoreName, new CancellationToken());
            //await this._SyncServer.ProcessDeltasAsync(DeltaProcessorName, deltas);
        }
        [HttpGet("Fetch")]
        public async Task<IEnumerable<IDelta>> Fetch(Guid startindex, string identity)
        {
            string name = GetHeader("DeltaStoreName");
            IEnumerable<IDelta> enumerable = await _SyncServer.GetDeltasAsync(name, startindex, identity, new CancellationToken());
            return enumerable;

        }

    }
}
