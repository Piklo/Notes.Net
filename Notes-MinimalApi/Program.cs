using Notes_MinimalApi;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Login;
using Notes_MinimalApi.Notes;
using Notes_MinimalApi.Register;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<DatabaseAccess>();
        builder.Services.AddScoped<HashedPasswordsProvider>();

        builder.Services.AddAuthentication(Constants.AuthSchema)
            .AddCookie(Constants.AuthSchema);

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

        app.MapPost("/register", RegisterEndpoint.HandleRegister)
            .AllowAnonymous();

        app.MapGet("/getNotes", GetNotesEndpoint.HandleGetNotes)
            .RequireAuthorization(Constants.LoggedInPolicyName);

        app.Run();
    }
}