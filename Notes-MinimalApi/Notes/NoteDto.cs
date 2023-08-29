namespace Notes_MinimalApi.Notes;

internal sealed class NoteDto
{
    public Guid Id { get; }
    public string Value { get; }

    public NoteDto(string id, string value)
    {
        Id = new Guid(id);
        Value = value;
    }
}