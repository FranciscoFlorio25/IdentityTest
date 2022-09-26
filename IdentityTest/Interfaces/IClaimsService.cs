using IdentityTest.Web.ViewModels;

namespace IdentityTest.Web.Interfaces
{
    public interface IClaimsService
    {
        Task AddClaim(string userId, string claimType, string claimValue);
        Task DeleteClaim(string userId, string claimType);

        Task<UserClaimViewModel> GetUserClaim(string userId);

        Task<CreateClaimViewModel> CreateClaim(string userId);

        Task<ClaimDeleteConfirmation> GetToBeDeleted(string userId, string claimType);
    }
}
