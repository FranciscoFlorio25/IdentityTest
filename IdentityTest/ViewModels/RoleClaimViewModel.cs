namespace IdentityTest.Web.ViewModels
{
	public class RoleClaimViewModel
	{
        public string RoleId { get; set; }
        public IEnumerable<RoleClaimDTO>? Claims { get; set; }
    }
}
