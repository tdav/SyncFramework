
namespace BIT.Data.Sync.Client
{
    public interface ISyncFrameworkLink
    {
        IDeltaProcessor DeltaProcessor { get; }
        IDeltaStore DeltaStore { get; }
        ISyncFrameworkClient SyncFrameworkClient { get; }


    }
}