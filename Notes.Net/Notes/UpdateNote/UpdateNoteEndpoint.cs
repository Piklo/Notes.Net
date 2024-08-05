using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.Notes.UpdateNote;

internal static class UpdateNoteEndpoint
{
    public static void MapUpdateNoteEndpoint(this WebApplication app)
    {
        app.MapPost("/updateNote", HandleAsync)
            .RequireAuthorization(Constants.LoggedInPolicyName);
    }

    private static async Task<UpdateNoteResponse> HandleAsync(HttpContext context, UpdateNoteDto note, DatabaseAccess databaseAccess)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.UserId)?.Value;

        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new UpdateNoteResponse { Status = UpdateNoteStatus.Failed };
        }

        using var connection = databaseAccess.Connect();

        var res = await connection.ExecuteAsync("UPDATE Notes SET Value = @Value WHERE Id = @Id AND UserId = @UserId", new { Value = note.Value, Id = note.Id, UserId = userId });

        if (res == 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new UpdateNoteResponse { Status = UpdateNoteStatus.ZeroAffected };
        }

        return new UpdateNoteResponse { Status = UpdateNoteStatus.Success };
    }
}
