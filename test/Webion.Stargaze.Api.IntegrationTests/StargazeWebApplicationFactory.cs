using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.PostgreSql;
using Webion.Stargaze.Api.IntegrationTests.Api.Client;
using Webion.Stargaze.Api.IntegrationTests.Seeders;
using Webion.Stargaze.Auth.Settings;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.IntegrationTests;

public sealed class StargazeWebApplicationFactory : WebApplicationFactory<StargazeApiAssemblyMarker>
{
    private static readonly PostgreSqlContainer Postgres;

    public StargazeApi Api => new(CreateClient());

    static StargazeWebApplicationFactory()
    {
        Postgres = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithCleanUp(true)
            .Build();

        Postgres.StartAsync().Wait();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Randomizer.Seed = new Random(1);

        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<StargazeDbContext>();
            services.RemoveAll<DbContextOptions<StargazeDbContext>>();
            services.RemoveAll<IDbContextFactory<StargazeDbContext>>();

            services.AddDbContextFactory<StargazeDbContext>(options =>
            {
                var conn = $"{Postgres.GetConnectionString()};Include Error Detail=true";
                options.UseNpgsql(conn, b =>
                {
                    b.MigrationsAssembly("Webion.Stargaze.Pgsql.Migrations");
                });
            }, ServiceLifetime.Scoped);
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        var db = host.Services.GetRequiredService<StargazeDbContext>();
        db.Database.Migrate();

        db.Seed<UsersSeeder>();
        db.Seed<CompaniesSeeder>();
        db.Seed<ProjectsSeeder>();
        db.Seed<TimePackagesSeeder>();

        return host;
    }

    protected override void ConfigureClient(HttpClient client)
    {
        var token = GetJwt(UsersSeeder.Stefano.Id);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public override async ValueTask DisposeAsync()
    {
        await Postgres.DisposeAsync();
        await base.DisposeAsync();
    }

    private string GetJwt(Guid id)
    {
        var jwtSettings = Server.Services.GetRequiredService<IOptions<JwtSettings>>().Value;
        var token = new JwtSecurityToken(
            claims:
            [
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            ],
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Issuer,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                algorithm: SecurityAlgorithms.HmacSha512Signature
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}