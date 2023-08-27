using Notes_MinimalApi.Database;
using Notes_MinimalApi.Login;
using Notes_MinimalApi.Notes;
using Notes_MinimalApi.Register;

internal sealed class Program
{
    internal const string AuthSchema = "cookie";
    internal const string PolicyName = "logged in";
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<DatabaseAccess>();
        builder.Services.AddScoped<HashedPasswordsProvider>();

        builder.Services.AddAuthentication(AuthSchema)
            .AddCookie(AuthSchema);

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyName, builder =>
            {
                builder.RequireAuthenticatedUser()
                .AddAuthenticationSchemes(AuthSchema);
            });
        });

        var app = builder.Build();
        app.MapPost("/login", LoginEndpoint.HandleLogin)
            .AllowAnonymous();

        app.MapPost("/register", RegisterEndpoint.HandleRegister)
            .AllowAnonymous();

        app.MapGet("/getNotes", GetNotesEndpoint.HandleGetNotes)
            .RequireAuthorization(PolicyName);

        app.Run();
    }
}