namespace Notes_MinimalApi.User.Login;

internal sealed class LoginUserData
{
    public required string Id { get; init; }
    public required string PasswordHash { get; init; }
}