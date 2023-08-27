using Microsoft.AspNetCore.Authentication;
using Notes_MinimalApi.Login;
using Notes_MinimalApi.Register;
using System.Security.Claims;

const string AuthSchema = "cookie";
const string PolicyName = "logged in";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(AuthSchema)
    .AddCookie(AuthSchema);
//.AddCookie(AuthSchema, options =>
//{
//    options.Events.OnRedirectToAccessDenied = context =>
//    {
//        context.Response.StatusCode = StatusCodes.Status403Forbidden;
//        return Task.CompletedTask;
//    };

//    options.Events.OnRedirectToLogin = context =>
//    {
//        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//        return Task.CompletedTask;
//    };
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyName, builder =>
    {
        builder.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(AuthSchema);
    });
});


var app = builder.Build();

app.MapPost("/register", (RegisterUserDto userDto, HttpContext context) =>
{

    return "account created";
}).AllowAnonymous();

app.MapPost("/login", async (LoginUserDto userDto, HttpContext context) =>
{
    if (!userDto.IsAuthenticated)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return "failed to login";
    }

    var claims = new List<Claim>()
    {
        new Claim("userLogin", userDto.Login)
    };

    var identity = new ClaimsIdentity(claims, AuthSchema);
    var user = new ClaimsPrincipal(identity);
    await context.SignInAsync(user);

    return "logged in";
}).AllowAnonymous();

app.MapGet("/getNotes", (HttpContext context) =>
{
    return "notes";
}).RequireAuthorization(PolicyName);

app.Run();
