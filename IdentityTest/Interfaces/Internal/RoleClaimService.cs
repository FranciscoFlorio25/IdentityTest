using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityTest.Web.Interfaces.Internal
{
    public class RoleClaimService : IClaimRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleClaimService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task AddClaim(string roleId, string claimType, string claimValue)
        {
            IdentityRole role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(roleId));
            Claim claim = new Claim(claimType, claimValue);
            await _roleManager.AddClaimAsync(role, claim);
        }

        public async Task<CreateRoleClaimViewModel> CreateClaim(string roleId)
        {
            var create = new CreateRoleClaimViewModel();
            create.RoleId = roleId;
            return create;
        }

        public async Task DeleteClaim(string roleId, string claimType)
        {
            IdentityRole role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(roleId));
            var claims = await _roleManager.GetClaimsAsync(role);

            Claim claim = claims.First(x => x.Type == claimType);

            await _roleManager.RemoveClaimAsync(role, claim);
        }

        public async Task<RoleClaimDeleteConfirmation> GetToBeDeleted(string roleId, string claimType)

        {
            RoleClaimDeleteConfirmation ToBeRemove = new();
            IdentityRole role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(roleId));

            ToBeRemove.RoleName = role.Name;
            ToBeRemove.RoleId = role.Id;
            ToBeRemove.ClaimType = claimType;


            return ToBeRemove;
        }

        public async Task<RoleClaimViewModel> GetRoleClaim(string roleId)
        {
            RoleClaimViewModel model = new();

            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(roleId));
            var roleClaims = getClaim(role.Id);

            model.Claims = await roleClaims;
            model.RoleId = role.Id;


            return model;
        }


        private async Task<IEnumerable<RoleClaimDTO>> getClaim(string roleId)
        {
            var role = await _roleManager.Roles.SingleAsync(x => x.Id.Equals(roleId));

            var claimList = new List<RoleClaimDTO>();
            var claims = await _roleManager.GetClaimsAsync(role);
            string subject;

            foreach (Claim claim in claims)
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


                claimList.Add(new RoleClaimDTO(subject, claim.Issuer, claim.Type, claim.Value));
            }


            return claimList;
        }
    }
}
