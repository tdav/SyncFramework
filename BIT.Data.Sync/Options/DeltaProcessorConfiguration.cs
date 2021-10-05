namespace BIT.Data.Sync.Options
{
    public class DeltaProcessorConfiguration
    {

        public DeltaProcessorConfiguration()
        {

        }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string DeltaStoreType { get; set; }
    }
}
