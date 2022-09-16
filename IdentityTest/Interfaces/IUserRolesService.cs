using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Web.Interfaces
{
    public interface IUserRolesService
    {
        Task<IEnumerable<IdentityRole>> getAll();
        Task<IdentityRole> get(string id);
        Task CreateRole(string roleName);
        Task DeleteRole(string id);
        Task UpdateRole(string id, string name);
        Task<ApplicationUser> GetUser(string UserId);
        Task<string> CurrentUserRole(string userId);
        Task AddToRole(string idUser, string idRole, string userRoleName);
        Task RemoveFromRole(string idUser, string idRole);
        Task<string[]> getroleNames();
    }
}
