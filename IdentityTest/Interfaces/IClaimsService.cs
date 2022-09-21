using IdentityTest.Models;
using System.Security.Claims;

namespace IdentityTest.Web.Interfaces
{
    public interface IClaimsService
    {
        Task AddClaim(ClaimsPrincipal User, string claimType, string claimValue);
        Task DeleteClaim(ClaimsPrincipal User, string claimValues);
    }
}
