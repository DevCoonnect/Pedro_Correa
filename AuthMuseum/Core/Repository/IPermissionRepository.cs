using AuthMuseum.Domain.Entities;

namespace AuthMuseum.Core.Repository;

public interface IPermissionRepository : IRepository<Permission>
{
    public Task<(bool Success, List<Permission> ParsedPermissions, List<string> InvalidPermissions)> 
        TryGetPermissionsByIdAsync(IEnumerable<string> permissions);
    public Task<List<Permission>> GetPermissionsAsync();
}