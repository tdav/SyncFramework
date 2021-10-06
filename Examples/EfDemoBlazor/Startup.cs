using BIT.Data.Sync;
using BIT.Data.Sync.Options;
using BIT.Data.Sync.Server;
using BIT.Data.Sync.Server.Extensions;
using BIT.EfCore.Sync;
using BIT.EfCore.Sync.DeltaProcessors;
using BIT.EfCore.Sync.DeltaStores;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SyncFrameworkTests.EF.Sqlite;
using SyncFrameworkTests.EF.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfDemoBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllers();
           

            IConfigurationSection ConfigMemoryDeltaStore1 = Configuration.GetSection("DeltaStore:MemoryDeltaStore1");
            IConfigurationSection ConfigMemoryDeltaStore2 = Configuration.GetSection("DeltaStore:MemoryDeltaStore2");

            services.Configure<DeltaStoreSettings>("DemoApp", ConfigMemoryDeltaStore1);
            services.Configure<DeltaStoreSettings>("MemoryDeltaStore2", ConfigMemoryDeltaStore2);

            List<DeltaStoreConfigurationOptions> DeltaStores = new List<DeltaStoreConfigurationOptions>();
            DeltaStoreConfigurationOptions MemoryDeltaStore1 = new DeltaStoreConfigurationOptions(typeof(EFDeltaStore), "DemoApp");
            DeltaStoreConfigurationOptions MemoryDeltaStore2 = new DeltaStoreConfigurationOptions(typeof(MemoryDeltaStore), "MemoryDeltaStore2");

            DeltaStores.Add(MemoryDeltaStore1);
            DeltaStores.Add(MemoryDeltaStore2);

            List<DeltaStoreConfigurationOptions> DeltaProcessors = new List<DeltaStoreConfigurationOptions>();
            DeltaStoreConfigurationOptions MemoryDeltaProcessor1 = new DeltaStoreConfigurationOptions(typeof(MemoryDeltaProcessor), "MemoryDeltaStore1");
            DeltaStoreConfigurationOptions MemoryDeltaProcessor2 = new DeltaStoreConfigurationOptions(typeof(EFDeltaProcessor), "MemoryDeltaStore2");
            DeltaProcessors.Add(MemoryDeltaProcessor1);
            DeltaProcessors.Add(MemoryDeltaProcessor2);
            services.AddDataStoreTypes(DeltaStores.ToArray(), DeltaProcessors.ToArray());


            services.AddSingleton(typeof(ConsoleEventService));
            services.AddScoped<ISyncServer, SyncServerBase>();

           
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
           
        }
    }
}
