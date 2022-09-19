﻿using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
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
            //quizas validaciones por aca estarian buenas

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role.Name))
            {
                return View();
            }
            await _userRoles.UpdateRole(id, role.Name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _userRoles.DeleteRole(id);
            return View();
        }

        [HttpGet]     
        public async Task<IActionResult> EditUserRoles(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRoles(string id, UserRoleViewModel model)
        {
            model = await _userRoles.GetRoles(id);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(model.RoleId))
            {
                return View(model);
            }


            await _userRoles.RemoveFromRole(id, model.RoleId);
            
            return View(model);
        }
    }
}
