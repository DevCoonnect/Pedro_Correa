using AuthMuseum.Core.Options;
using AuthMuseum.Core.Repository;
using AuthMuseum.Core.Repository.Impl;
using AuthMuseum.Core.Services;
using AuthMuseum.Core.Services.Impl;
using AuthMuseum.Infra.Database;
using AuthMuseum.IoC;
using Microsoft.EntityFrameworkCore;
/*
 * Dúvidas:
 *   - Quando a pessoa loga, n deveria expirar qualquer token anterior existente?
 *   - No refresh-token, devemos gerar novas permissões? Algo dinâmico talvez?
 */
/*
 * Pendente:
 * 
 * Modulo de permissões tá com um problema (olhando pra db e não tá indo os valores pra lá)
 * Permissionamento
 *
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddPostgresDbContext();
builder.Services.AddRedis();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<IArtRepository, ArtRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IArtService, ArtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddAuthInjection(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
    await db.Database.MigrateAsync();
    await db.SaveChangesAsync();
}

await app.RunAsync();