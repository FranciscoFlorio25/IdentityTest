using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleClaimController : Controller
    {
        private readonly IClaimRolesService _claimsService;

        public RoleClaimController(IClaimRolesService RoleclaimsService)
        {
            _claimsService = RoleclaimsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            var user = await _claimsService.GetRoleClaim(Id);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string Id)
        {
            var create = await _claimsService.CreateClaim(Id);
            return View(create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Id, CreateClaimViewModel model)
        {
            string errorMessage = "";


            if (string.IsNullOrEmpty(model.ClaimType))
            {
                errorMessage = "Claim type must be filled";
            }

            if (string.IsNullOrEmpty(model.ClaimValue))
            {
                errorMessage = "Claim value must be filled";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                var create = await _claimsService.CreateClaim(Id);
                return View(create);
            }
            await _claimsService.AddClaim(Id, model.ClaimType, model.ClaimValue);
            return RedirectToAction("Index", "Roles");
        }

        [Route("/RoleClaim/RoleClaimDelete/{roleId}/{claimType}")]
        [HttpGet]
        public async Task<IActionResult> RoleClaimDelete(string roleId, string claimType)
        {

            return View(await _claimsService.GetToBeDeleted(roleId, claimType));
        }

        [Route("/RoleClaim/RoleClaimDelete/{roleId}/{claimType}")]
        [HttpPost]
        public async Task<IActionResult> RoleClaimDelete(string roleId, string claimType, RoleClaimDeleteConfirmation model)
        {
            model.ClaimType = claimType;

            if (string.IsNullOrEmpty(model.ClaimType))
            {
                return RedirectToAction("Index");
            }
            await _claimsService.DeleteClaim(roleId, model.ClaimType);
            return RedirectToAction("Index", "Roles");
        }
    }
}
