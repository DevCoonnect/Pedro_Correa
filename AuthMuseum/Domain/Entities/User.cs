using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuthMuseum.Domain.Enums;
using Newtonsoft.Json;

namespace AuthMuseum.Domain.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    [JsonIgnore]
    public required string Password { get; set; }
    [JsonIgnore]
    public string? RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime? RefreshTokenExpiry { get; set; }
    public Profiles Profile { get; set; }
    [JsonIgnore]
    public List<Permission> IndividualPermissions { get; set; } = new();
    public IEnumerable<Permissions> Permissions =>
        IndividualPermissions
            .Select(p => p.Value)
            .ToImmutableList();
}