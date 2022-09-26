namespace IdentityTest.Web.ViewModels
{
    public class UserClaimViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<ClaimDTO>? Claims { get; set; }
    }
}
