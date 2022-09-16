namespace IdentityTest.Web.ViewModels
{
	public record UserRoleDTO
		(
			string userEmail,
			string userRoleName,
			string[] rolesToBe
		);
}
