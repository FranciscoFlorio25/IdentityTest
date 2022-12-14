using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IUserRolesService _userRoles;

        public RolesController(IUserRolesService userRoles)
        {
            _userRoles = userRoles;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _userRoles.GetAll();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesDTO role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                ViewBag.ErrorMessage = "Name must be filled";
                return View();
            }
            await _userRoles.CreateRole(role.Name);
            return RedirectToAction("Index");
        }
        [HttpGet]

        public async Task<IActionResult> Update(string id)
        {
            var toUpdate = await _userRoles.Get(id);
            if (toUpdate != null)
            {
                return View(toUpdate);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, RolesDTO role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                ViewBag.ErrorMessage = "Name must be filled";
                var toUpdate = await _userRoles.Get(id);
                return View(toUpdate);
            }
            await _userRoles.UpdateRole(id, role.Name);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> RoleDelete(string id)
        {
            return View(await _userRoles.RoleToBeDeleted(id));
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id, ConfirmRoleToDelete model)
        {
            await _userRoles.DeleteRole(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string id)
        {

            if (string.IsNullOrEmpty(id))
            {

                return View(await _userRoles.GetRoles(id));
            }

            var userRole = await _userRoles.GetRoles(id);
            return View(userRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRoles(string id, UserRoleViewModel model)
        {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(model.RoleId))
            {

                return View(model);
            }

            await _userRoles.AddToRole(id, model.RoleId);
            var userRole = await _userRoles.GetRoles(id);
            return View(userRole);
        }
        [Route("Account/{userId}/Roles/{roleId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteUserRoles(string roleId, string userId)
        {

            var userRole = await _userRoles.GetToBeRemove(userId, roleId);
            return View(userRole);
        }

        [Route("Account/{userId}/Roles/{roleId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRoles(string roleId, string userId,
            ConfirmRemoveUserRole model)
        {
            await _userRoles.RemoveFromRole(userId, roleId);
            return RedirectToAction("Index", "Account");
        }
    }
}
