using Azure.Storage.Blobs;
using BLOBuploaderAPI.Helpers;
using BLOBuploaderAPI.Helpers.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(x => new BlobServiceClient(
                 builder.Configuration.GetSection(Config.BlobConfigurationSectionName)
                .GetValue<String>(Config.ConnectionStringValueName)));
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<IBlobHelper, BlobHelper>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
