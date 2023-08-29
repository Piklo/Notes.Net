namespace Notes_MinimalApi.Notes.GetNotes;

public sealed class Note
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Value { get; init; }
}