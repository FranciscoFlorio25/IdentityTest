using IdentityTest.Models;
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
        Task RegisterUserAsync(string password, string email);

        Task LoginUserAsync(string email, string password);

        Task LogOutAsync();

        Task UserDelete(string id);
        Task<ApplicationUser> GetFromId(string id);
        Task UpdateUser(string id, string email);

        Task UpdatePassword(string id, string password);
    }
}
