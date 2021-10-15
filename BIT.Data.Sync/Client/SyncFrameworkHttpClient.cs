//using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIT.Data.Sync.Client
{
    //TODO Review cancellation token implementation https://docs.devexpress.com/CodeRushForRoslyn/119690/static-code-analysis/analyzers-library/crr0035-no-cancellation-token-parameter-in-the-asynchronous-method
    public class SyncFrameworkHttpClient : ISyncFrameworkClient
    {
        HttpClient _httpClient;
        public string EndpointName { get; }
        public SyncFrameworkHttpClient(HttpClient httpClient,string DeltaStoreId)
        {
            this.EndpointName = DeltaStoreId;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("DeltaStoreName", DeltaStoreId);
            _httpClient.DefaultRequestHeaders.Add("DeltaProcessorName", DeltaStoreId);
            this.EndpointName = DeltaStoreId;
        }
        public SyncFrameworkHttpClient(string BaseAddress, string DeltaStoreId):this(new HttpClient() { BaseAddress=new Uri(BaseAddress)},DeltaStoreId)
        {
           
        }
        public virtual async Task PushAsync(IEnumerable<IDelta> Deltas, CancellationToken cancellationToken  = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var json = JsonConvert.SerializeObject(Deltas);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("/Sync/Push", data, cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task<List<Delta>> FetchAsync(Guid startindex, string identity, CancellationToken cancellationToken = default)
        {
            var QueryParams = new Dictionary<string, string>();
            QueryParams.Add(nameof(startindex), startindex.ToString());
            QueryParams.Add(nameof(identity), identity);
            cancellationToken.ThrowIfCancellationRequested();
            var query = HttpUtility.ParseQueryString("");
            foreach (KeyValuePair<string, string> CurrentParam in QueryParams)
            {
                query[CurrentParam.Key] = CurrentParam.Value;
            }
            var reponse = await _httpClient.GetStringAsync($"/Sync/Fetch?{query.ToString()}").ConfigureAwait(false);
            List<Delta> Deltas = JsonConvert.DeserializeObject<List<Delta>>(reponse);
            return Deltas;

        }

    }
}
