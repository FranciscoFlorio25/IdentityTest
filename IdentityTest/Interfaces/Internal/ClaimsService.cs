using IdentityTest.Models;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddClaim(string userId, string claimType, string claimValue)
        {
            ApplicationUser user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            Claim claim = new Claim(claimType, claimValue);
            await _userManager.AddClaimAsync(user, claim);
        }

        public async Task DeleteClaim(string userId, string claimValues)
        {
            ApplicationUser user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            var claims = await _userManager.GetClaimsAsync(user);

            string[] claimValuesArray = claimValues.Split(";");

            string claimType = claimValuesArray[0],
                claimValue = claimValuesArray[1],
                claimIssuer = claimValuesArray[2];

            Claim claim = claims.First(x =>
            x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer);

            await _userManager.RemoveClaimAsync(user, claim);
        }

        public async Task<ClaimDeleteConfirmation> GetToBeDeleted(string userId, string claimValues)
        {
            ClaimDeleteConfirmation ToBeRemove = new();
            ApplicationUser user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));


            string[] claimValuesArray = claimValues.Split(";");

            string claimType = claimValuesArray[0],
                claimValue = claimValuesArray[1];


            ToBeRemove.UserEmail = user.Email;
            ToBeRemove.UserId = user.Id;
            ToBeRemove.ClaimValues = claimValues;
            ToBeRemove.ClaimType = claimType;
            ToBeRemove.ClaimValue = claimValue;


            return ToBeRemove;
        }

        private async Task<IEnumerable<ClaimDTO>> getClaim(string userId)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));

            var claimList = new List<ClaimDTO>();
            var claims =await _userManager.GetClaimsAsync(user);
            string subject;

            foreach(Claim claim in claims)
            {
                if (claim.Subject != null)
                {
                    if (!string.IsNullOrEmpty(claim.Subject.Name))
                    {
                        subject = claim.Subject.Name;
                    }
                    else
                    {
                        subject = "No Subject";
                    }

                }
                else
                {
                    subject = "No Subject";
                }


                claimList.Add(new ClaimDTO(subject, claim.Issuer, claim.Type, claim.Value));
            }


            return claimList;
        }

        public async Task<UserClaimViewModel> GetUserClaim(string userId)
        {
            UserClaimViewModel model = new();

            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            var userClaims = getClaim(userId);

            model.Claims = await userClaims;
            model.UserId = user.Id;


            return model;
        }

        public async Task<CreateClaimViewModel> CreateClaim(string userId)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(userId));
            var create = new CreateClaimViewModel();
            create.UserId = userId;
            return create;
        }


    }
}
