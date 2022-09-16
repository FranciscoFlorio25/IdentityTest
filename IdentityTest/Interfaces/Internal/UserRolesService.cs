using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace IdentityTest.Web.Interfaces.Internal
{
    internal class UserRolesService : IUserRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesService(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

        public async Task<ApplicationUser> GetUser(string UserId)
        {
            return await _userManager.Users.SingleAsync(x => x.Id.Equals(UserId));
        }

        public async Task AddToRole(string idUser, string idRole, string userRoleName)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(idRole)
            && x.Name.Equals(userRoleName));
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(idUser));
            bool isInRole = await _userManager.IsInRoleAsync(user, role.Name);

            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(user, role.Name);

            }
        }
        public async Task RemoveFromRole(string idUser, string idRole)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(idRole));
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(idUser));
            bool isInRole = await _userManager.IsInRoleAsync(user, role.Name);

            if (isInRole)
            {
                await _userManager.RemoveFromRoleAsync(user,role.Name);

            }
        }

        public async Task<string> CurrentUserRole(string userId)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return "No role yet";
            }
            return role;
        }

        public async Task<string[]> getroleNames()
        {
            var roles = await getAll();
            return roles.Select(x => x.Name).ToArray();
        }
    }
}
