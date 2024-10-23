using AuthMuseum.Domain.Entities;
using AuthMuseum.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthMuseum.Core.Repository.Impl;

public class PermissionRepository(PostgresDbContext context) : BaseRepository<Permission>(context), IPermissionRepository
{
    
    private readonly DbSet<Permission> _permissions = context.Set<Permission>();
    
    public async Task<(bool Success, List<Permission> ParsedPermissions, List<string> InvalidPermissions)> TryGetPermissionsByIdAsync(IEnumerable<string> permissionsList)
    {
        var permissions = await _permissions.Where(p => permissionsList.Contains(p.Value.ToString())).ToListAsync();
        var invalidPermissions = permissionsList.Except(permissions.Select(p => p.Value.ToString())).ToList();
        
        return (invalidPermissions.Count == 0, permissions, invalidPermissions);
    }

    public async Task<List<Permission>> GetPermissionsAsync() => await _permissions.ToListAsync();

}