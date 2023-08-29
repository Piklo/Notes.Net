using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Register;

internal sealed class RegisterResponse
{
    public required RegisterStatus Status { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RegisterStatus StatusText => Status;
}