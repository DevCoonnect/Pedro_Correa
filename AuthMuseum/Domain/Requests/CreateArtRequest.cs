using System.ComponentModel.DataAnnotations;

namespace AuthMuseum.Domain.Requests;

public record CreateArtRequest
{
    [StringLength(100, ErrorMessage = "Title is too long. Max characters is {1}.")]
    public required string Name { get; init;  }
    
    [StringLength(100, ErrorMessage = "Author name is too long. Max characters is {1}.")]
    public string? Author { get; init; }
    
    [StringLength(500, ErrorMessage = "Description is too long. Max characters is {1}.")]
    public string? Description { get; init; }
    
    public DateTime Date { get; init; }
}