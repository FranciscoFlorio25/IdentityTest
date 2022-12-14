namespace IdentityTest.Web.ViewModels
{
    public class UserIndexViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public string? UserAddress { get; set; }
        public IList<string>? UserRoles { get; set; }



    }
}
