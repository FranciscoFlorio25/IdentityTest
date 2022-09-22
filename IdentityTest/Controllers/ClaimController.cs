using IdentityTest.Models;
using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;

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

            if (string.IsNullOrEmpty(model.ClaimType) || string.IsNullOrEmpty(model.ClaimValue))
            {
                return RedirectToAction("Index");
            }

            await _claimsService.AddClaim(Id, model.ClaimType, model.ClaimValue);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id,string claimValues)
        {
            
            return View(await _claimsService.GetToBeDeleted(Id, claimValues));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id, ClaimDeleteConfirmation model)
        {
            if (string.IsNullOrEmpty(model.ClaimValue))
            {
                return RedirectToAction("Index");
            }
            await _claimsService.DeleteClaim(Id, model.ClaimValues);
            return RedirectToAction("Index");
        }

    }
}
