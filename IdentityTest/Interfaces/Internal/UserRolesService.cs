using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Web.Interfaces.Internal
{
    internal class UserRolesService : IUserRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
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

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

        }
        public async Task<IdentityRole> Get(string id)
        {
            return await _roleManager.Roles.SingleAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<IdentityRole>> GetAll()
        {
            return await _roleManager.Roles.ToListAsync();
        }


        private async Task<ApplicationUser> GetUser(string UserId)
        {
            return await _userManager.Users.SingleAsync(x => x.Id.Equals(UserId));
        }

        private async Task<IEnumerable<RolesDTO>> UserRoles(string userId)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesDTO = new List<RolesDTO>();

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    rolesDTO.Add(new RolesDTO(role.Id, role.Name));
                }
            }

            return rolesDTO;
        }

        public async Task<UserRoleViewModel> GetRoles(string id)
        {
            var user = await GetUser(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesDTO = new List<RolesDTO>();
            var currentRoles = await UserRoles(id);

            foreach (var role in roles)
            {
                rolesDTO.Add(new RolesDTO(role.Id, role.Name));
            }

            UserRoleViewModel userRolesDto = new();
            userRolesDto.UserId = user.Id;
            userRolesDto.UserEmail = user.Email;
            userRolesDto.CurrentRoles = currentRoles;
            userRolesDto.RoleList = rolesDTO;

            return userRolesDto;
        }
        public async Task UpdateRole(string id, string name)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(id));
            role.Name = name;
            await _roleManager.UpdateAsync(role);

        }
        public async Task AddToRole(string idUser, string idRole)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(idRole));
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
                await _userManager.RemoveFromRoleAsync(user, role.Name);

            }
        }

        public async Task<ConfirmRemoveUserRole> GetToBeRemove(string idUser, string idRole)
        {
            var user = await GetUser(idUser);
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(idRole));

            ConfirmRemoveUserRole UserRoleToRemove = new();
            UserRoleToRemove.UserEmail = user.Email;
            UserRoleToRemove.UserId = user.Id;
            UserRoleToRemove.RoleToRemove = role.Name;
            UserRoleToRemove.RoleId = role.Id;

            return UserRoleToRemove;
        }

        public async Task<ConfirmRoleToDelete> RoleToBeDeleted(string Id)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(Id));
            ConfirmRoleToDelete getToDelete = new();
            getToDelete.RoleId = role.Id;
            getToDelete.RoleName = role.Name;


            return getToDelete;
        }
    }
}
