using Microsoft.AspNetCore.Identity;

namespace Notes_MinimalApi.Register;

internal sealed class RegisterUserDto
{
    public string Login { get; }
    public string Email { get; }
    public string Password { get; }

    public RegisterUserDto(string login, string email, string password)
    {
        Login = login;
        Email = email;
        var hasher = new PasswordHasher<string>();
        Password = hasher.HashPassword(login, password);
    }
}