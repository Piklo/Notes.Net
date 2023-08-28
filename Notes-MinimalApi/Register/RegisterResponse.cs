using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Register;

internal sealed class RegisterResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RegisterStatus Status { get; init; }
}