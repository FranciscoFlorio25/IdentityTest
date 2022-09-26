using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _aplicationUserService.GetAll();

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [Route("/Account/UserIndex/{id}")]
        [HttpGet]
        public async Task<IActionResult> UserIndex(string id)
        {
            var user = await _aplicationUserService.GetFromId(id);

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel user)
        {

            if (!string.IsNullOrWhiteSpace(await _aplicationUserService.LoginUserAsync(user.Email, user.Password)))
            {
                ViewBag.ErrorMessage = await _aplicationUserService.LoginUserAsync(user.Email, user.Password);
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
        public async Task<IActionResult> Register(UserViewModel user, string returnUrl)
        {
            string errorMessage = "";

            if (user.Email == null)
            {
                errorMessage = "Email should not be null";
            }
            if (user.PhoneNumber == null)
            {
                errorMessage = "PhoneNumber should not be null";
            }

            if (user.Password == null)
            {
                errorMessage = "Password should not be null";
            }

            if (user.Password == null)
            {
                errorMessage = "Password should not be null";

            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }

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
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return View(await _aplicationUserService.ToBeDeleted(id));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, ConfirmAccountDelete model)
        {
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
            string errorMessage = "";
            if (user.UserEmail == null)
            {
                errorMessage = "Email should not be null";
            }
            if (user.UserPhoneNumber == null)
            {
                errorMessage = "PhoneNumber should not be null";
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                user = await _aplicationUserService.getUserToUpdate(id);
                return View(user);
            }
            await _aplicationUserService.UpdateUser(id, user);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _aplicationUserService.GetChangePassword(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordViewModel user)
        {

            string errorMessage = "";

            if (user.Password == null)
            {
                errorMessage = "Password must be filled";
            }

            if (user.ConfirmPassword == null)
            {
                errorMessage = "You must confirm the password";
            }

            if (!string.IsNullOrWhiteSpace(user.Password) && !user.Password.Equals(user.ConfirmPassword))
            {
                errorMessage = "Password must match";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                user = await _aplicationUserService.GetChangePassword(id);
                return View(user);
            }

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                await _aplicationUserService.UpdatePassword(id, user.Password);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
