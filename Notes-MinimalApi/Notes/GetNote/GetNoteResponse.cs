using System.Text.Json.Serialization;

namespace Notes_MinimalApi.Notes.GetNote;

internal sealed class GetNoteResponse
{
    public required GetNoteStatus Status { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GetNoteStatus StatusText => Status;

    public NoteDto? Note { get; init; }
}
