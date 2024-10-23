using Newtonsoft.Json.Converters;

namespace AuthMuseum.Domain.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum Permissions
{
    ALL,
    CAN_CREATE_PROFILE,
    CAN_RETRIEVE_PROFILES,
    CAN_UPDATE_PROFILE,
    CAN_DELETE_PROFILE,
    CAN_CREATE_ARTS,
    CAN_RETRIEVE_ARTS,
    CAN_UPDATE_ARTS,
    CAN_DELETE_ARTS,
}