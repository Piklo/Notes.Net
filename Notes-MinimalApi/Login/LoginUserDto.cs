using Microsoft.AspNetCore.Identity;

namespace Notes_MinimalApi.Login;

internal sealed class LoginUserDto
{
    public string Login { get; }
    public string Password { get; }
    public bool IsAuthenticated { get; }

    public LoginUserDto(string login, string password)
    {
        Login = login;
        Password = password;
        var hasher = new PasswordHasher<string>();
        var hashedPassword = GetHashedPassword();
        var res = hasher.VerifyHashedPassword(login, hashedPassword, password);

        IsAuthenticated = res != PasswordVerificationResult.Failed;
    }

    private static string GetHashedPassword()
    {
        return "AQAAAAIAAYagAAAAEENZJJe787d3SsvwPIBqorpsOC7Tue/Q954tTz60olBw1wJHS1AG5DeFfSa90sIJQA==";
    }
}