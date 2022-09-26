namespace IdentityTest.Web.ViewModels
{
    public record RoleClaimDTO(string? ClaimSubject, string ClaimIssuer, string ClaimType, string ClaimValue);
}
