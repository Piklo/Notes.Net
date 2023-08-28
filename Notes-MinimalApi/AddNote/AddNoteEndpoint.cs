using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.AddNote;

public static class AddNoteEndpoint
{
    public static void MapAddNoteEndpoint(this WebApplication app)
    {
        app.MapPost("/addNote", HandleAsync)
            .RequireAuthorization(Constants.LoggedInPolicyName);
    }

    private static async Task<AddNoteResponse> HandleAsync(HttpContext context, AddNoteDto note, DatabaseAccess databaseAccess)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.UserId)?.Value;

        if (userId is null)
        {
            return new AddNoteResponse() { Status = AddNoteStatus.Failed };
        }

        using var connection = databaseAccess.Connect();

        await connection.ExecuteAsync("INSERT INTO Notes (Id, UserId, Value) VALUES (@Id, @UserId, @Value)", new { Id = Guid.NewGuid(), UserId = userId, Value = note.Value });

        return new AddNoteResponse() { Status = AddNoteStatus.Success };
    }
}
