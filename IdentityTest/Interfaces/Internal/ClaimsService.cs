using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityTest.Web.Interfaces.Internal
{
    internal class ClaimsService : IClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddClaim(ClaimsPrincipal User, string claimType, string claimValue)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            Claim claim = new Claim(claimType, claimValue);
            await _userManager.AddClaimAsync(user, claim);
        }

        public async Task DeleteClaim(ClaimsPrincipal User, string claimValues)
        {
            ApplicationUser user = await _userManager.GetUserAsync (User);

            string[] claimValuesArray = claimValues.Split(";");

            string claimType = claimValuesArray[0],
                claimValue = claimValuesArray[1],
                claimIssuer = claimValuesArray[2];

            Claim claim = User.Claims.First(x =>
            x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer);

            await _userManager.RemoveClaimAsync(user, claim);
        }
    }
}
