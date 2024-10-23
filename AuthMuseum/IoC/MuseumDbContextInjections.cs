using AuthMuseum.Infra.Database;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace AuthMuseum.IoC;

public static class MuseumDbContextInjections
{
    public static IServiceCollection AddPostgresDbContext(this IServiceCollection services)
    {
        _ = services.AddDbContextPool<PostgresDbContext>((sp, options) =>
        {
            if (sp.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                _ = options.EnableDetailedErrors();
                _ = options.EnableSensitiveDataLogging();
            }

            _ = options.UseApplicationServiceProvider(sp);
            _ = options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());

            _ = options.UseNpgsql(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres"),
                postgresOptions => { postgresOptions.CommandTimeout(30); });

            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(new RedisConfiguration
        {
            Name = "Redis_Museum_Database",
            Hosts = [
                new()
                {
                    Host = "redis", Port = 6379
                }
            ]
        });
        return services;
    }
    
}