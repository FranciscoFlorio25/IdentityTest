namespace IdentityTest.Web.ViewModels
{
	public record ClaimDTO(string? ClaimSubject, string ClaimIssuer, string ClaimType, string ClaimValue);
}