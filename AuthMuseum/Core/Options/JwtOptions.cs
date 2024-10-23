using Microsoft.Extensions.Options;

namespace AuthMuseum.Core.Options;

public class JwtOptions : IOptions<JwtOptions>
{
    public string SecurityKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int SessionTime { get; init; }
    public int DaysToExpire { get; init; }
    
    public JwtOptions Value => this;
}