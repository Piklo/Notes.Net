using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.Notes.GetNote;

internal static class GetNoteEndpoint
{
    public static void MapGetNoteEndpoint(this WebApplication app)
    {
        app.MapGet("/getNote", GetNoteEndpoint.HandleAsync)
            .RequireAuthorization(Constants.LoggedInPolicyName);
    }

    private static async Task<GetNoteResponse> HandleAsync(HttpContext context, string id, DatabaseAccess databaseAccess)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.UserId)?.Value;

        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new GetNoteResponse() { Status = GetNoteStatus.Failed };
        }

        using var connection = databaseAccess.Connect();

        var note = await connection.QuerySingleOrDefaultAsync<NoteDto>("SELECT Id, Value FROM Notes WHERE Id = @Id AND UserId = @UserId", new { Id = id, @UserId = userId });

        if (note is null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return new GetNoteResponse() { Status = GetNoteStatus.NoteNotFound };
        }

        return new GetNoteResponse() { Status = GetNoteStatus.Success, Note = note };
    }
}
