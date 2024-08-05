namespace Notes_MinimalApi.User.Register;

internal sealed class RegisterUserDto
{
    public required string Login { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}