using DDD.Host;
using Microsoft.AspNetCore;

var app =
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>().Build();

app.Run();