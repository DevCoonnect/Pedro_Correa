using AuthMuseum.Core.Common;
using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Requests;

namespace AuthMuseum.Core.Services;

public interface IArtService
{
    Task<Result<Art>> CreateArt(CreateArtRequest request);
    Task<Result<Art>> GetArtById(long id);
    Task<Result<Art>> DeleteArt(long id);
    Task<Result<List<Art>>> GetArts();
}