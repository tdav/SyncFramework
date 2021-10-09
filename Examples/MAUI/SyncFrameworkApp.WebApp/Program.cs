using BIT.Data.Sync.Options;
using BIT.Data.Sync.Server;
using BIT.Data.Sync.Server.Extensions;
using BIT.EfCore.Sync;
using BIT.EfCore.Sync.DeltaProcessors;
using BIT.EfCore.Sync.DeltaStores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SyncFrameworkApp.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


//SYNC
IConfigurationSection ConfigMemoryDeltaStore1 = builder.Configuration.GetSection("DeltaStore:MemoryDeltaStore1");
IConfigurationSection ConfigMemoryDeltaStore2 = builder.Configuration.GetSection("DeltaStore:MemoryDeltaStore2");

builder.Services.Configure<DeltaStoreSettings>("DemoApp", ConfigMemoryDeltaStore1);
builder.Services.Configure<DeltaStoreSettings>("MemoryDeltaStore2", ConfigMemoryDeltaStore2);

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
builder.Services.AddDataStoreTypes(DeltaStores.ToArray(), DeltaProcessors.ToArray());



builder.Services.AddScoped<ISyncServer, SyncServerBase>();

//SYNC


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();
app.Run();
