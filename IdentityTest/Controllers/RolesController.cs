using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IdentityTest.Web.Controllers
{
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
            var roles = await _userRoles.getAll();
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
                return View() ;
            }
            await _userRoles.CreateRole(role.Name);
            return RedirectToAction("Index");
        }
        [HttpGet]

        public async Task<IActionResult> Update(string id)
        {
            var toUpdate = await _userRoles.get(id);
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
            //quizas validaciones por aca estarian buenas

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role.Name))
            {
                return View();
            }
            await _userRoles.UpdateRole(id,role.Name);
            return RedirectToAction("Index");
        }
    }
}
