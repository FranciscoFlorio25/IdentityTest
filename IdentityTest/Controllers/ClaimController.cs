using IdentityTest.Models;
using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;

namespace IdentityTest.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClaimController : Controller
    {
        private readonly IClaimsService _claimsService;

        public ClaimController(IClaimsService claimsService)
        {
            _claimsService = claimsService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            var user = await _claimsService.GetUserClaim(Id);
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
            return RedirectToAction("Index");
        }
        [Route("/Claim/Delete/{userId}/{claimType}")]
        [HttpGet]
        public async Task<IActionResult> Delete(string userId,string claimType)
        {
            
            return View(await _claimsService.GetToBeDeleted(userId, claimType));
        }
        [Route("/Claim/Delete/{userId}/{claimType}")]
        [HttpPost]
        public async Task<IActionResult> Delete(string userId,string claimType,ClaimDeleteConfirmation model)
        {
            model.ClaimType = claimType;

            if (string.IsNullOrEmpty(model.ClaimType))
            {
                return RedirectToAction("Index");
            }
            await _claimsService.DeleteClaim(userId, model.ClaimType);
            return RedirectToAction("Index", "Account");
        }

    }
}
