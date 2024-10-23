using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record BadRequestError(string message) : Error(message)
{
}
