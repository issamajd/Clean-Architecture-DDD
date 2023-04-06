using Twinkle.Identity.Controllers;
using Serilog;
using Twinkle.Identity;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web app {AppName}", AppDomain.CurrentDomain.FriendlyName);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog();
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();