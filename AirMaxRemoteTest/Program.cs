using AirMaxRemoteTest.Data;
using AirMaxRemoteTest.Helpers;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<VehicleDataDecoder>();

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("./Log.txt")
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(log);
log.Information("Airmax Remote Test - Initialised");

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

app.Run();
