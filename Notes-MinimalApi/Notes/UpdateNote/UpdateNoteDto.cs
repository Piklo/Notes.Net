namespace Notes_MinimalApi.Notes.UpdateNote;

internal sealed class UpdateNoteDto
{
    public required Guid Id { get; init; }
    public required string Value { get; init; }
}
