using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuthMuseum.Domain.Enums;

namespace AuthMuseum.Domain.Entities;

[Table("permissions")]
public record Permission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    public required Permissions Value { get; set; }
}