using Notes_MinimalApi;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Notes.AddNote;
using Notes_MinimalApi.Notes.GetNote;
using Notes_MinimalApi.Notes.GetNotes;
using Notes_MinimalApi.Notes.Logout;
using Notes_MinimalApi.Notes.RemoveNote;
using Notes_MinimalApi.Notes.UpdateNote;
using Notes_MinimalApi.User.Login;
using Notes_MinimalApi.User.Register;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DatabaseAccess>();

builder.Services.AddAuthentication(Constants.AuthSchema)
    .AddCookie(Constants.AuthSchema, options =>
    {
        options.Events.OnRedirectToAccessDenied = c =>
        {
            c.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Cookie.Name = "session";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.LoggedInPolicyName, builder =>
    {
        builder.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.AuthSchema)
        .RequireClaim(ClaimConstants.UserId);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapLoginEndpoint();
app.MapLogoutEndpoint();
app.MapRegisterEndpoint();
app.MapAddNoteEndpoint();
app.MapGetNotesEndpoint();
app.MapGetNoteEndpoint();
app.MapUpdateNoteEndpoint();
app.MapRemoveNoteEndpoint();

app.Run();