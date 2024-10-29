using System.Security.Claims;
using AuthMuseum.Core.Requirements;
using AuthMuseum.Domain.Enums;
using FastEnumUtility;
using Microsoft.AspNetCore.Authorization;

namespace AuthMuseum.Core.Handlers;

public class RequirePermissionsAuthorizationHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<RequirePermissionsAttribute>
{

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirePermissionsAttribute requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            context.Fail(new AuthorizationFailureReason(this, $"Cannot get {nameof(HttpContext)} inside {nameof(RequirePermissionsAuthorizationHandler)}."));
            return Task.CompletedTask;
        }

        if (httpContext.User.Identity?.IsAuthenticated != true)
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not authenticated."));
            return Task.CompletedTask;
        }
        
        var claims = httpContext.User.Claims.ToList();
        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToHashSet();
        
        var hasAllPermissions = permissions.Contains(Permissions.ALL.ToString()) ||
                                claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == Profiles.ADMIN.ToString();

        if (!hasAllPermissions)
        {  
            var missingPermissions = requirement.Permissions
                .Select(p => p.ToString())
                .Except(permissions)
                .ToList();

            if (missingPermissions.Count != 0)
            {
                context.Fail(new AuthorizationFailureReason(this, $"Missing permissions: ({ string.Join(", ", missingPermissions) })."));
            }
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}