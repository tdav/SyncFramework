using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BIT.Data.Sync.Client
{
  
    public class SyncFrameworkHttpClient : ISyncFrameworkClient
    {
        HttpClient _httpClient;
        public string DeltaStoreId { get; }
        public SyncFrameworkHttpClient(HttpClient httpClient,string NodeId)
        {
            this.DeltaStoreId = NodeId;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("NodeId", NodeId);
          
            this.DeltaStoreId = NodeId;
        }
        public SyncFrameworkHttpClient(string BaseAddress, string DeltaStoreId):this(new HttpClient() { BaseAddress=new Uri(BaseAddress)},DeltaStoreId)
        {
           
        }
        public virtual async Task PushAsync(IEnumerable<IDelta> Deltas, CancellationToken cancellationToken  = default)
        {
           
            try
            {
                List<Delta> toserialzie = new List<Delta>();
                foreach (IDelta delta in Deltas)
                {
                    toserialzie.Add(new Delta(delta));
                }
                cancellationToken.ThrowIfCancellationRequested();

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<Delta>));
                MemoryStream msObj = new MemoryStream();
                js.WriteObject(msObj, toserialzie);
                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);
                string jsonDeltas = sr.ReadToEnd();

                var data = new StringContent(jsonDeltas, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("/Sync/Push", data, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
          
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

            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(reponse)))
            {

                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<Delta>));
                List<Delta> Deltas = (List<Delta>)deserializer.ReadObject(ms);

                return Deltas;
            }

            //List<Delta> Deltas = JsonConvert.DeserializeObject<List<Delta>>(reponse);
            return null;

        }

    }
}
