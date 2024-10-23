using AuthMuseum.Core.Common;
using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Repository;
using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Errors;
using AuthMuseum.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AuthMuseum.Core.Services.Impl;

public class ArtService(IArtRepository artRepository) : IArtService
{
    public async Task<Result<Art>> CreateArt(CreateArtRequest request)
    {
        var art = request.ToArt();
        await artRepository.AddAsync(art);
        await artRepository.SaveChangesAsync();
        
        return art;
    }

    public async Task<Result<Art>> GetArtById(long id)
    {
        var art = await artRepository.GetArtByIdAsync(id);
        if (art == null)
        {
            return new NotFoundError($"Art with Id {id} not found!");
        }

        return art;
    }

    public async Task<Result<Art>> DeleteArt(long id)
    {
        var art = await artRepository.GetArtByIdAsync(id);
        if (art == null)
        {
            return new NotFoundError($"Art with Id {id} not found!");
        }

        artRepository.Delete(art);
        await artRepository.SaveChangesAsync();

        return art;
    }

    public async Task<Result<List<Art>>> GetArts()
    {
        var arts = await artRepository.GetArtsAsync();
        return arts;
    }
}