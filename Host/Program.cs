using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDD.Host;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web app {AppName}", AppDomain.CurrentDomain.FriendlyName);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseSerilog();
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);
builder.Host.ConfigureContainer<ContainerBuilder>(startup.ConfigureContainer);

var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();