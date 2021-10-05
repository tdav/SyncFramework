using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Client
{
    public static class ISyncFrameworkLinkExtensions
    {

        public static async Task<List<Delta>> FetchAsync(this ISyncFrameworkLink instance, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var LastDetalIndex = await instance.DeltaStore.GetLastProcessedDeltaAsync(cancellationToken).ConfigureAwait(false);
            var query = new Dictionary<string, string>
            {

#pragma warning disable CRRSP06 // A misspelled word has been found
                ["startindex"] = LastDetalIndex.ToString(),
#pragma warning restore CRRSP06 // A misspelled word has been found
                ["identity"] = instance.DeltaStore.Identity,

            };

            return await instance.SyncFrameworkClient.FetchAsync(query, cancellationToken).ConfigureAwait(false);
        }
        public static async Task PullAsync(this ISyncFrameworkLink instance, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Deltas = await instance.FetchAsync(cancellationToken).ConfigureAwait(false);
            if (Deltas.Any())
            {
                await instance.DeltaProcessor.ProcessDeltasAsync(Deltas, cancellationToken).ConfigureAwait(false);
                Guid index = Deltas.Max(d => d.Index);
                await instance.DeltaStore.SetLastProcessedDeltaAsync(index, cancellationToken).ConfigureAwait(false);
            }

        }
        public static async Task PushAsync(this ISyncFrameworkLink instance, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var LastPushedDelta = await instance.DeltaStore.GetLastPushedDeltaAsync(cancellationToken).ConfigureAwait(false);
            var Deltas = await instance.DeltaStore.GetDeltasAsync(LastPushedDelta,cancellationToken).ConfigureAwait(false);
            if (Deltas.Any())
            {
                var Max = Deltas.Max(d => d.Index);
                await instance.SyncFrameworkClient.PushAsync(Deltas, cancellationToken).ConfigureAwait(false);
                await instance.DeltaStore.SetLastPushedDeltaAsync(Max,cancellationToken).ConfigureAwait(false);
            }



        }
    }
}