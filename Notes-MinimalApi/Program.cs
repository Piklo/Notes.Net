using Notes_MinimalApi;
using Notes_MinimalApi.AddNote;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Login;
using Notes_MinimalApi.Notes;
using Notes_MinimalApi.Register;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DatabaseAccess>();
builder.Services.AddScoped<HashedPasswordsProvider>();

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
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.LoggedInPolicyName, builder =>
    {
        builder.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(Constants.AuthSchema);
    });
});

var app = builder.Build();
app.MapPost("/login", LoginEndpoint.HandleLogin)
    .AllowAnonymous();

app.MapRegisterEndpoint();

app.MapAddNoteEndpoint();

app.MapGet("/getNotes", GetNotesEndpoint.HandleGetNotes)
    .RequireAuthorization(Constants.LoggedInPolicyName);

app.Run();