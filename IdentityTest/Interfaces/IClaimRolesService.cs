using IdentityTest.Web.ViewModels;

namespace IdentityTest.Web.Interfaces
{
	public interface IClaimRolesService
	{
        Task AddClaim(string roleId, string claimType, string claimValue);
        Task DeleteClaim(string roleId, string claimValues);

        Task<RoleClaimViewModel> GetRoleClaim(string roleId);

        Task<CreateRoleClaimViewModel> CreateClaim(string roleId);

        Task<RoleClaimDeleteConfirmation> GetToBeDeleted(string roleId, string claimValues);
    }
}
