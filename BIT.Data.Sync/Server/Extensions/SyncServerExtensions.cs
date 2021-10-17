
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace BIT.Data.Sync.Server.Extensions
{
    public static class SyncServerExtensions
    {
        //public static void AddSyncServer(this IServiceCollection services,IDeltaStore[] deltaStores,IDeltaProcessor[] deltaProcessor)
        //{

        //}
        //public static void AddDataStoreTypes(this IServiceCollection services, DeltaStoreConfigurationOptions[] DeltaStoreOptions, DeltaStoreConfigurationOptions[] DeltaProcessorsOptions)
        //{
        //    Dictionary<string, Type> DeltaStores = GetTypes(DeltaStoreOptions);
        //    Dictionary<string, Type> DeltaProcessors = GetTypes(DeltaProcessorsOptions);
        //    //services.AddSingleton(new ReflectionService(DeltaStores, DeltaProcessors));
        //}

        //private static Dictionary<string, Type> GetTypes(DeltaStoreConfigurationOptions[] ConfigurationOptions)
        //{
        //    Dictionary<string, Type> Types = new Dictionary<string, Type>();
        //    foreach (var type in ConfigurationOptions)
        //    {
        //        Types.Add(type.ConfigurationName, type.Type);
        //    }

        //    return Types;
        //}


    }
}
