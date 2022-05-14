namespace Assignment_DotNet6.Entities
{
    public enum UserAuthenticationStatus
    {
        NotFound = 0,
        Authenticated,
        InvalidPassword
    }
    public enum UserSignUpStatus
    {
        Created = 1,
        DuplicateUsername,
        DuplicateEmail,
        InvalidPassword
    }
}
