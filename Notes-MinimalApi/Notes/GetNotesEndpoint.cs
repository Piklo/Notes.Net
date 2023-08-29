using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.Notes;

internal static class GetNotesEndpoint
{
    public static void MapGetNotesEndpoint(this WebApplication app)
    {
        app.MapGet("/getNotes", GetNotesEndpoint.HandleAsync)
            .RequireAuthorization(Constants.LoggedInPolicyName);
    }
    public static async Task<GetNotesResponse> HandleAsync(HttpContext context, DatabaseAccess databaseAccess)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.UserId)?.Value;

        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new GetNotesResponse() { Status = GetNotesStatus.Failed };
        }

        var connection = databaseAccess.Connect();

        var notes = (await connection.QueryAsync<NoteDto>("SELECT Id, Value FROM Notes WHERE UserId = @UserId", new { UserId = userId })).ToArray();

        return new GetNotesResponse() { Status = GetNotesStatus.Success, Notes = notes };
    }
}
