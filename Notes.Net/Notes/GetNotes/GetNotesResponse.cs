namespace Notes_MinimalApi.Notes.GetNotes;

internal sealed class GetNotesResponse
{
    public required GetNotesStatus Status { get; init; }

    public NoteDto[] Notes { get; init; } = Array.Empty<NoteDto>();
}
