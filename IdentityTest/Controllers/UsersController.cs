using IdentityTest.Web.Interfaces;
using IdentityTest.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IApplicationUserService _aplicationUserService;


        public UsersController(IApplicationUserService aplicationUserService)
        {
            _aplicationUserService = aplicationUserService;
        }

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
        public async Task<IActionResult> Register(UserDTO user)
        {

            await _aplicationUserService.RegisterUserAsync(user.Password, user.Email);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await _aplicationUserService.logOutAsync();
            return RedirectToAction("Login");
        }


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
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _aplicationUserService.GetFromId(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UserDTO user)
        {
            //quizas validaciones por aca estarian buenas

            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(user.Email) 
                || string.IsNullOrEmpty(user.Password))
            {
                return View();
            }
            await _aplicationUserService.UpdateUser(id, user.Email);
            return RedirectToAction("Index");
        }

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
            //quizas validaciones por aca estarian buenas

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrEmpty(user.PasswordConfirm))
            {

                return View();
            }
            await _aplicationUserService.UpdatePassword(id, user.Password);
            return RedirectToAction("Index");
        }
    }
}
