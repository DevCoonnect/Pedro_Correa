using AuthMuseum.Domain.Entities;

namespace AuthMuseum.Core.Repository;

public interface IArtRepository : IRepository<Art>
{
    public Task<Art?> GetArtByIdAsync(long id);
    public Task<List<Art>> GetArtsAsync();
}