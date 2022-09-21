using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserService _aplicationUserService;


        public AccountController(IApplicationUserService aplicationUserService)
        {
            _aplicationUserService = aplicationUserService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _aplicationUserService.GetAll();

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserDTO user)
        {


            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return View();
            }
            await _aplicationUserService.LoginUserAsync(user.Email, user.Password);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(UserDTO user,string returnUrl)
        {
            
            await _aplicationUserService.RegisterUserAsync(user);

            if (!string.IsNullOrEmpty(returnUrl))
                {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await _aplicationUserService.LogOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            await _aplicationUserService.UserDelete(id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _aplicationUserService.getUserToUpdate(id);



            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UserUpdateViewModel user)
        {

            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(user.UserEmail))
            {
                return View();
            }
            await _aplicationUserService.UpdateUser(id, user);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _aplicationUserService.GetFromId(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, UserDTO user)
        { 

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrEmpty(user.PasswordConfirm))
            {

                return View();
            }
            if(!user.Password.Equals(user.PasswordConfirm))
            {
                return View();
            }
            await _aplicationUserService.UpdatePassword(id, user.Password);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
