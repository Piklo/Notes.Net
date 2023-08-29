using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Notes;

internal sealed class GetNotesResponse
{
    public required GetNotesStatus Status { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GetNotesStatus StatusText => Status;

    public NoteDto[] Notes { get; init; } = Array.Empty<NoteDto>();
}
