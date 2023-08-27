namespace Notes_MinimalApi.Register;

internal static class RegisterEndpoint
{
    public static async Task<string> HandleRegister(RegisterUserDto userDto, HttpContext context)
    {
        return "account created";
    }
}
