namespace IdentityTest.Web.ViewModels
{
	public record UserRoleViewModel
	{

        public string UserEmail { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<string> currentRoles { get; set; }
        public IEnumerable<RolesDTO> RoleList { get; set; }
    }

}
