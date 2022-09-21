namespace IdentityTest.Web.ViewModels
{
    public record UserDTO (
        string UserName,
        string Email,
        string Password,
        string PasswordConfirm,
        string? FirstName,
        string? LastName,
        string? Address,
        string? PhoneNumber
        );
}
