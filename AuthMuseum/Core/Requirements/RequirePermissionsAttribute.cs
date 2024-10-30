using AuthMuseum.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AuthMuseum.Core.Requirements;

public class RequirePermissionsAttribute(params Permissions[] permissions) : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public Permissions[] Permissions { get; } = permissions;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}