using System.Text.Json.Serialization;

namespace Notes_MinimalApi.AddNote;

internal sealed class AddNoteResponse
{
    public required AddNoteStatus Status { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AddNoteStatus StatusText => Status;
}
