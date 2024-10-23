using Newtonsoft.Json.Converters;

namespace AuthMuseum.Domain.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum Profiles
{
    ADMIN,
    KEEPER,
    NONE
}