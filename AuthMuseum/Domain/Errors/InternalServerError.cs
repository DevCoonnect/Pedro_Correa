using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record InternalServerError(string message) : Error(message)
{
    public const string DefaultInternalServerErrorMessage = "An error has occurred!";
}
