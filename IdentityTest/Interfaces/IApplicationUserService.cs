using IdentityTest.Models;
using IdentityTest.Web.ViewModels;

namespace IdentityTest.Web.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task RegisterUserAsync(UserViewModel user);

        Task<string> LoginUserAsync(string email, string password);

        Task LogOutAsync();

        Task UserDelete(string id);
        Task<ConfirmAccountDelete> ToBeDeleted(string id);
        Task<UserIndexViewModel> GetFromId(string id);
        Task UpdateUser(string id, UserUpdateViewModel model);

        Task UpdatePassword(string id, string password);

        Task<UserUpdateViewModel> getUserToUpdate(string Id);
        Task<ChangePasswordViewModel> GetChangePassword(string Id);

    }
}
