
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.Data.Sync.Server
{
    public class SyncServerBase: ISyncServer
    {


        IDictionary<string, ISyncServerNode> _Nodes;
        public SyncServerBase()
        {

        }
        public SyncServerBase(IDictionary<string, ISyncServerNode> Nodes)
        {
            this._Nodes = Nodes;
        }
        public async Task<IEnumerable<IDelta>> GetDeltasAsync(string name, Guid startindex, string identity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var Node = _Nodes[name];
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

            var Node = this._Nodes[Name];
            if (Node != null)
            {
                return Node.ProcessDeltasAsync(deltas, cancellationToken);
            }
            return null;

        }
        public Task SaveDeltasAsync(string name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var Node = _Nodes[name];

            if (Node != null)
            {
                return Node.SaveDeltasAsync(deltas, cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
    //public class SyncServerBase : ISyncServerNode
    //{
    //    IOptionsSnapshot<DeltaStoreSettings> _options;
    //    ReflectionService _deltaStoreFactory;
    //    public SyncServerBase(IOptionsSnapshot<DeltaStoreSettings> options, ReflectionService deltaStoreFactory)
    //    {
    //        this._options = options;
    //        this._deltaStoreFactory = deltaStoreFactory;
    //    }
    //    public async Task<IEnumerable<IDelta>> GetDeltasAsync(string name, Guid startindex, string identity, CancellationToken cancellationToken)
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();
    //        var Options = this._options.Get(name);
    //        var deltaStore = this._deltaStoreFactory.CreateDeltaStore(Options);
    //        if (deltaStore != null)
    //        {

    //            return await deltaStore.GetDeltasFromOtherNodes(startindex, identity, cancellationToken).ConfigureAwait(false); 
    //        }

    //        IEnumerable<IDelta> result = new List<IDelta>();
    //        return result;
    //    }

    //    public Task ProcessDeltasAsync(string Name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();
    //        var Options = this._options.Get(Name);
    //        var Processor = this._deltaStoreFactory.CreateDeltaProcessor(Options);
    //        return Processor.ProcessDeltasAsync(deltas, cancellationToken);

    //    }

    //    public Task SaveDeltasAsync(string name, IEnumerable<IDelta> deltas, CancellationToken cancellationToken)
    //    {
    //        cancellationToken.ThrowIfCancellationRequested();
    //        var Options = this._options.Get(name);
    //        var deltaStore = this._deltaStoreFactory.CreateDeltaStore(Options);

    //        if (deltaStore != null)
    //        {
    //            return deltaStore.SaveDeltasAsync(deltas,cancellationToken);
    //        }
    //        return Task.CompletedTask;
    //    }
    //}

}
