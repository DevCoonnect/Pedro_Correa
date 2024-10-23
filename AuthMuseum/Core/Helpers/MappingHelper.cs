using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Requests;

namespace AuthMuseum.Core.Helpers;

public static class MappingHelper
{
    public static Art ToArt(this CreateArtRequest request)
    {
        return new Art
        {
            Name = request.Name,
            Author = request.Author,
            Description = request.Description,
            Date = request.Date
        };
    }
}