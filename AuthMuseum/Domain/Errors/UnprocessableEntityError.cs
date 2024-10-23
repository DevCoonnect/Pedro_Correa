using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record UnprocessableEntityError(string message) : Error(message)
{
}
