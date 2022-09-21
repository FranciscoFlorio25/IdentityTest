namespace IdentityTest.Web.ViewModels
{
	public class ConfirmRemoveUserRole
	{	
		public string UserId { get; set; }
		public string UserEmail { get; set; }
		public string RoleId { get; set; }
		public string RoleToRemove { get; set; }
	}
}
