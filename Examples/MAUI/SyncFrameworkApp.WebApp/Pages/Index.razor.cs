using BIT.Data.Sync;
using BIT.EfCore.Sync;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using SyncFrameworkApp.Controls.Data;
using SyncFrameworkTests.EF.Sqlite;
using SyncFrameworkTests.EF.SqlServer;

namespace SyncFrameworkApp.WebApp.Pages
{
    public partial class Index
    {
        public OrmContext OrmContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Identity { get; set; }
        public string BlogName { get; set; }

        public Radzen.Blazor.RadzenTabs TabControl { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ServiceCollection ServiceCollection = new ServiceCollection();


            List<DeltaGeneratorBase> DeltaGenerators = new List<DeltaGeneratorBase>();
            DeltaGenerators.Add(new SqlServerDeltaGenerator());
            DeltaGenerators.Add(new SqliteDeltaGenerator());

            // SqlServerServiceCollection.AddEfSynchronization((options) => { options.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=EFDemoDeltas{Identity};Trusted_Connection=True;"); }, "https://localhost:1705", "DemoApp", DeltaGenerators);
            ServiceCollection.AddEfSynchronization((options) => { options.UseSqlite($"Data Source=Databases\\{Identity}Deltas.db"); }, "DemoApp", NavigationManager.BaseUri, DeltaGenerators);
            ServiceCollection.AddEntityFrameworkSqlite();
            ServiceCollection.AddSingleton<ISyncIdentityService>(new SyncIdentityService(Identity));

            ServiceCollection.AddSingleton<IServiceCollection>(ServiceCollection);

            DbContextOptionsBuilder<OrmContext> dbContextOptions = new DbContextOptionsBuilder<OrmContext>();
            dbContextOptions.UseSqlite($"Data Source=Databases\\{Identity}Data.db");

            this.OrmContext = new OrmContext(dbContextOptions.Options, ServiceCollection, new SyncIdentityService(Identity));


        }
    }
}
