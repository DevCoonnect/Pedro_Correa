using AuthMuseum.Domain.Entities;
using AuthMuseum.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthMuseum.Core.Repository.Impl;

public class ArtRepository(PostgresDbContext context) : BaseRepository<Art>(context), IArtRepository
{
    
    private readonly DbSet<Art> _arts = context.Set<Art>();

    public async Task<Art?> GetArtByIdAsync(long id)
    {
        return await _arts.FindAsync(id);
    }

    public async Task<List<Art>> GetArtsAsync()
    {
        return await _arts.ToListAsync();
    }

    public List<Art> GetArts()
    {
        return [.. _arts];
    }
    
}