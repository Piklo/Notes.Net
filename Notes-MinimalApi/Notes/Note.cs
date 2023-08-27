namespace Notes_MinimalApi.Notes;

public sealed class Note
{
    public required Guid Id { get; init; }
    public required Guid OwnerId { get; init; }
    public string Value { get; init; }
}