namespace Notes_MinimalApi.Notes.GetNote;

internal sealed class GetNoteResponse
{
    public required GetNoteStatus Status { get; init; }

    public NoteDto? Note { get; init; }
}
