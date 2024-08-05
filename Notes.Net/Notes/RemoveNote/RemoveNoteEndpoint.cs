using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.Notes.RemoveNote;

internal static class RemoveNoteEndpoint
{
    public static void MapRemoveNoteEndpoint(this WebApplication app)
    {
        app.MapPost("/removeNote", HandleAsync)
            .RequireAuthorization(Constants.LoggedInPolicyName);
    }

    public static async Task<RemoveNoteResponse> HandleAsync(HttpContext context, RemoveNoteDto note, DatabaseAccess databaseAccess)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.UserId)?.Value;

        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new RemoveNoteResponse { Status = RemoveNoteStatus.Failed };
        }

        using var connection = databaseAccess.Connect();

        var res = await connection.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId", new { Id = note.Id, UserId = userId });

        if (res == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new RemoveNoteResponse { Status = RemoveNoteStatus.ZeroAffected };
        }

        return new RemoveNoteResponse { Status = RemoveNoteStatus.Success };
    }
}
