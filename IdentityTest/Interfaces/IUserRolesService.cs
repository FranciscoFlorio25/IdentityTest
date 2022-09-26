using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Web.Interfaces
{
    public interface IUserRolesService
    {
        Task<IEnumerable<IdentityRole>> GetAll();
        Task<IdentityRole> Get(string id);
        Task CreateRole(string roleName);
        Task DeleteRole(string id);
        Task UpdateRole(string id, string name);
        Task AddToRole(string idUser, string idRole);
        Task RemoveFromRole(string idUser, string idRole);
        Task<UserRoleViewModel> GetRoles(string id);
        Task<ConfirmRemoveUserRole> GetToBeRemove(string idUser, string idRole);

        Task<ConfirmRoleToDelete> RoleToBeDeleted(string Id);
    }
}
