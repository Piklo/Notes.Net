namespace Notes_MinimalApi.Notes;

internal static class GetNotesEndpoint
{
    public static async Task<string> HandleGetNotes(HttpContext context)
    {
        return "notes";
    }
}
