using BIT.Data.Sync;
using BIT.Data.Sync.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BIT.EfCore.Sync
{
    public abstract class SyncFrameworkDbContext : DbContext, ISyncClientNode
    {
        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="SyncFrameworkDbContext" /> class. The
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />
        /// method will be called to configure the database (and other options) to be used for this context.
        /// </para>
        /// </summary>
        protected SyncFrameworkDbContext()
        {
           
        }


        public ISyncFrameworkClient SyncFrameworkClient { get; private set; }
        public IDeltaStore DeltaStore { get; private set; }
        public IDeltaProcessor DeltaProcessor { get; private set; }

        protected IServiceProvider serviceProvider;
        protected IServiceCollection _ServiceCollection;
        public string Identity { get; private set; }
        protected SyncFrameworkDbContext(DbContextOptions options, IServiceCollection ServiceCollection, string Identity) : base(options)
        {
            this.Identity = Identity;
            _ServiceCollection = ServiceCollection;
            _ServiceCollection.AddSingleton<IDeltaProcessor>(new EFDeltaProcessor(this));
            this.serviceProvider = _ServiceCollection.BuildServiceProvider();

        }
        public SyncFrameworkDbContext(DbContextOptions options, IServiceCollection ServiceCollection, ISyncIdentityService syncIdentityService) : this(options, ServiceCollection, syncIdentityService.Identity)
        {
   
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            this.DeltaStore = serviceProvider.GetService<IDeltaStore>();
            this.DeltaProcessor = serviceProvider.GetService<IDeltaProcessor>();
            this.SyncFrameworkClient = serviceProvider.GetService<ISyncFrameworkClient>();
            var IEFSyncFrameworkService = serviceProvider.GetService<IEFSyncFrameworkService>();
            IEFSyncFrameworkService.RegisterDeltaGenerators(serviceProvider);
            this.DeltaStore.SetIdentity(this.Identity);
            optionsBuilder.UseInternalServiceProvider(serviceProvider);
        }




    }
}
