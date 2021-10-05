using System;

namespace BIT.Data.Sync.Server.Extensions
{
    public class DeltaStoreConfigurationOptions
    {
        public Type Type { get; set; }
        public string ConfigurationName { get; set; }
        public DeltaStoreConfigurationOptions(Type type, string configurationName)
        {
            Type = type;
            ConfigurationName = configurationName;
        }
    }
}
