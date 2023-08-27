namespace Notes_MinimalApi.Login;

internal sealed class LoginUserDto
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}