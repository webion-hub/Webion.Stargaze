using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Webion.Stargaze.Pgsql.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var conn = builder.Configuration.GetConnectionString("db")!;

builder.Services.AddStargazeDbContext(conn);

var host = builder.Build();

host.Run();