
using BIT.Data.Sync;
using BIT.Data.Sync.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BIT.EfCore.Sync
{
    public static class SyncFrameworkExtensions
    {

        public static IServiceCollection AddEfSynchronization(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> DeltaContextOption, HttpClient httpClient, IEnumerable<DeltaGeneratorBase> deltaGenerators)
        {
            SyncFrameworkClient syncFrameworkClient = new SyncFrameworkClient(httpClient);
            return serviceCollection.AddEfSynchronization(DeltaContextOption, syncFrameworkClient, deltaGenerators);
        }
        public static IServiceCollection AddEfSynchronization(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> DeltaContextOption, string EndpointName, string ServerUrl, IEnumerable<DeltaGeneratorBase> deltaGenerators)
        {
            SyncFrameworkClient syncFrameworkClient = new SyncFrameworkClient(ServerUrl,EndpointName);
            return serviceCollection.AddEfSynchronization(DeltaContextOption, syncFrameworkClient, deltaGenerators);
        }
        public static IServiceCollection AddEfSynchronization(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> DeltaContextOption, ISyncFrameworkClient httpClient, IEnumerable<DeltaGeneratorBase> deltaGenerators)
        {

            serviceCollection.AddSingleton(typeof(ISyncFrameworkClient), httpClient);
            serviceCollection.AddScoped<IBatchExecutor, SyncFrameworkBatchExecutor>();
            serviceCollection.AddDbContext<DeltaDbContext>(DeltaContextOption);
            //serviceCollection.AddTransient<IDeltaStore, EFDeltaStore>();
            serviceCollection.AddSingleton<IDeltaStore, EFDeltaStore>();
            //serviceCollection.AddSingleton<IEFSyncFrameworkService, EFSyncFrameworkServiceBase>();
            var EfSyncFrameworkService = new EFSyncFrameworkServiceBase(deltaGenerators);
            serviceCollection.AddSingleton<IEFSyncFrameworkService>(EfSyncFrameworkService);
            return serviceCollection;

        }

    }
}
