using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTest.Web.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task RegisterUserAsync(UserDTO user);

        Task LoginUserAsync(string email, string password);

        Task LogOutAsync();

        Task UserDelete(string id);
        Task<UserIndexViewModel> GetFromId(string id);
        Task UpdateUser(string id, UserUpdateViewModel model);

        Task UpdatePassword(string id, string password);

        Task<UserUpdateViewModel> getUserToUpdate(string Id);
        Task<ChangePasswordViewModel> GetChangePassword(string Id);
    }
}
