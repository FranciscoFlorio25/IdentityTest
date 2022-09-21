using IdentityTest.Models;
using IdentityTest.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View(User?.Claims);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            
            if (string.IsNullOrEmpty(claimType) || string.IsNullOrEmpty(claimValue))
            {
                return RedirectToAction("Index");
            }

            await _claimsService.AddClaim(HttpContext.User, claimType,claimValue);
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(string claimValues)
        {
            if (string.IsNullOrEmpty(claimValues))
            {
                return RedirectToAction("Index");
            }
            await _claimsService.DeleteClaim(HttpContext.User, claimValues);
            return View();
        }

    }
}
