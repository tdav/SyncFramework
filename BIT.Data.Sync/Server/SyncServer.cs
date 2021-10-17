
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Server
{
    public class SyncServer: ISyncServer
    {


        IDictionary<string, ISyncServerNode> _Nodes;
        public SyncServer()
        {

        }
        public SyncServer(IDictionary<string, ISyncServerNode> Nodes)
        {
            this._Nodes = Nodes;
        }

        public IDictionary<string, ISyncServerNode> Nodes => _Nodes;

        public async Task<IEnumerable<IDelta>> GetDeltasAsync(string name, Guid startindex, string identity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Node = Nodes[name];
            if (Node != null)
            {
                return await Node.GetDeltasAsync(startindex, identity, cancellationToken).ConfigureAwait(false);
            }

            IEnumerable<IDelta> result = new List<IDelta>();
            return result;
        }
        public Task ProcessDeltasAsync(string Name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var Node = this.Nodes[Name];
            if (Node != null)
            {
                return Node.ProcessDeltasAsync(deltas, cancellationToken);
            }
            return null;

        }
        public Task SaveDeltasAsync(string name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var Node = Nodes[name];

            if (Node != null)
            {
                return Node.SaveDeltasAsync(deltas, cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
   

}
