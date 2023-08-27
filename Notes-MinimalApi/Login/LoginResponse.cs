using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Login;

internal sealed class LoginResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required LoginStatus Status { get; init; }
}
