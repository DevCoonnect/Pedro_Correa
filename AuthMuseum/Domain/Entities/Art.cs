using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthMuseum.Domain.Entities;

[Table("arts")]
public class Art
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [StringLength(100, ErrorMessage = "Title is too long. Max characters is {1}.")]
    public required string Name { get; set;  }
    
    [StringLength(100, ErrorMessage = "Author name is too long. Max characters is {1}.")]
    public string? Author { get; set; }
    
    [StringLength(500, ErrorMessage = "Description is too long. Max characters is {1}.")]
    public string? Description { get; set; }
    
    public DateTime? Date { get; set; }
}