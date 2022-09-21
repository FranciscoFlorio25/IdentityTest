namespace IdentityTest.Web.ViewModels
{
	public record UserRoleViewModel
	{

        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<RolesDTO>? CurrentRoles { get; set; }
        public IEnumerable<RolesDTO>? RoleList { get; set; }
    }

}
