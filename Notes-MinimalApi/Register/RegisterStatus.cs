namespace Notes_MinimalApi.Register;

internal enum RegisterStatus
{
    Failed,
    Success,
    EmailInUse,
    LoginInUse,
    InvalidPassword,
}