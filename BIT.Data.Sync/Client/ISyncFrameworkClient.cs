﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Client
{
    public interface ISyncFrameworkClient
    {
        
        Task<List<Delta>> FetchAsync(Dictionary<string, string> query, CancellationToken cancellationToken);
        Task PushAsync(IEnumerable<IDelta> Deltas, CancellationToken cancellationToken);
    }
}
