using BIT.Data.Sync.Client;
using Microsoft.AspNetCore.Components.Web;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using BIT.EfCore.Sync;
using EfDemoBlazor.Data;
using SyncFrameworkTests.EF.SqlServer;
using SyncFrameworkTests.EF.Sqlite;
using Microsoft.EntityFrameworkCore;
using BIT.Data.Sync;

namespace EfDemoBlazor.Pages
{
    public partial class Client:ComponentBase
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
        public int DeltaCount { get; set; }
        protected async override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender && this.OrmContext.Database.CanConnect())
            {
                this.RefreshData();
            }
        }
        public List<Person> Contacts = new List<Person>();
        private static Person GetBlog(string Name, string Title1, string Title2)
        {
            return new Person { Name = Name, Posts = { new Post { Title = Title1 }, new Post { Title = Title2 } } };
        }
        async void AddBlog(MouseEventArgs args)
        {
            
            await OrmContext.Database.EnsureCreatedAsync();

            Person entity = GetBlog(this.BlogName, $"{this.BlogName} Post 1", $"{this.BlogName} Post 2");

            OrmContext.Add(
            entity);
            await OrmContext.SaveChangesAsync();
            this.BlogName = string.Empty;

            RefreshData();

        }
        async void RefreshData()
        {
            this.DeltaCount = await OrmContext.DeltaStore.GetDeltaCountAsync(await OrmContext.DeltaStore.GetLastPushedDeltaAsync(default), new System.Threading.CancellationToken());
            Contacts = new List<Person>();
            var data = this.OrmContext.Contacts.ToList();
            this.Contacts.AddRange(data);
            this.StateHasChanged();
        }
        async void Pull(MouseEventArgs args)
        {
            await OrmContext.PullAsync();
            RefreshData();

        }
        async void Push(MouseEventArgs args)
        {
            await OrmContext.PushAsync();
            RefreshData();
        }
        async void InitDatabase(MouseEventArgs args)
        {
            await OrmContext.Database.EnsureDeletedAsync();
            await OrmContext.Database.EnsureCreatedAsync();
            await OrmContext.DeltaStore.PurgeDeltasAsync(default);
            RefreshData();
        }
    }
}
