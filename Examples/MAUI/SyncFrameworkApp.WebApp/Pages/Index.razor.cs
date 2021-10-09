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
       

        public Radzen.Blazor.RadzenTabs TabControl { get; set; }
        protected override async Task OnInitializedAsync()
        {
           
        }
        protected async override void OnAfterRender(bool firstRender)
        {

            base.OnAfterRender(firstRender);


            if (!firstRender)
                return;

            Identity = "Master";
            string DeltasCnx = $"Data Source=Databases\\{Identity}Deltas.db";
            string DataCnx = $"Data Source=Databases\\{Identity}Data.db";
            ServiceCollection ServiceCollection = new ServiceCollection();


            List<DeltaGeneratorBase> DeltaGenerators = new List<DeltaGeneratorBase>();
            DeltaGenerators.Add(new SqlServerDeltaGenerator());
            DeltaGenerators.Add(new SqliteDeltaGenerator());


            ServiceCollection.AddEfSynchronization((options) => { options.UseSqlite(DeltasCnx); }, "DemoApp", NavigationManager.BaseUri, DeltaGenerators);
            ServiceCollection.AddEntityFrameworkSqlite();
            ServiceCollection.AddSingleton<ISyncIdentityService>(new SyncIdentityService(Identity));

            ServiceCollection.AddSingleton<IServiceCollection>(ServiceCollection);

            DbContextOptionsBuilder<OrmContext> dbContextOptions = new DbContextOptionsBuilder<OrmContext>();
            dbContextOptions.UseSqlite(DataCnx);

            this.OrmContext = new OrmContext(dbContextOptions.Options, ServiceCollection, new SyncIdentityService(Identity));

            this.OrmContext.Database.EnsureCreated();
            if (this.OrmContext.Users.Count() == 0)
            {
                AddInitialData();
            }
            else
            {
                this.User.AddRange(this.OrmContext.Users);
            }


         
        }
        public List<User> User { get; set; } = new();

       

        private async Task AddInitialData()
        {

            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()}
            }
            });

            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() }
            }
            });

            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() }
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() }
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() }
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() }
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068", Id= Guid.NewGuid() },
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()}
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()}
            }
            });


            User.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Steven",
                LastName = "Checo",
                Email = "steven.checo.19@gmail.com",
                BirthDay = DateTime.Now,
                RegisterDate = DateTime.UtcNow,
                Contacts = new List<SyncFrameworkApp.Controls.Data.UserContact> {
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()},
                new UserContact {Address = "1810 wallace ave", Phones = "8096953068" , Id= Guid.NewGuid()}
            }
            });
            await this.OrmContext.AddRangeAsync(User);
            await this.OrmContext.SaveChangesAsync();
        }
    }
}
