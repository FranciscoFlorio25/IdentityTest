using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Web.Interfaces.Internal
{
    internal class UserRolesService : IUserRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesService(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRole(string roleName)
        {
            IdentityRole newRole = new() { Name = roleName };
            await _roleManager.CreateAsync(newRole);
        }

        public async Task DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.Roles.
                SingleAsync(x => x.Id.Equals(id));
           
            if(role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
           
        }

        public async Task<IdentityRole> get(string id)
        {
            return await _roleManager.Roles.SingleAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<IdentityRole>> getAll()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task UpdateRole(string id, string name)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(id));
            role.Name = name;
            await _roleManager.UpdateAsync(role);

        }
    }
}
