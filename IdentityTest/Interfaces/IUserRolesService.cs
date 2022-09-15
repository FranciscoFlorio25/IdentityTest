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
    }
}
