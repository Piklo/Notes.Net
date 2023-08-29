using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Login;

internal sealed class LoginResponse
{
    public required LoginStatus Status { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LoginStatus StatusText => Status;
}
