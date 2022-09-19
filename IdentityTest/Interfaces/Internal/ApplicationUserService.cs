using IdentityTest.Models;
using IdentityTest.Web.Interfaces;
using IdetityTest.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IdentityTest.Web.Controllers;
using System.Runtime.InteropServices;

namespace IdentityTest.Web
{
    internal class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UsersController> _logger;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
            ILogger<UsersController> logger, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await _userManager.Users.ToListAsync();

        }
        public async Task<ApplicationUser> GetFromId(string id)
        {
            return await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
        }

        public async Task RegisterUserAsync(string password, string email)
        {
            ApplicationUser newUser = new() {Email = email , EmailConfirmed=true, UserName = email };

            await _userManager.CreateAsync(newUser, password);

        }
        public async Task LoginUserAsync(string email, string password)
        {     
            var result = await _signInManager.PasswordSignInAsync(email,password,false,false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User Logged in");
            }
            if (result.IsNotAllowed)
            {
                _logger.LogInformation("User fail to sing in");
            }
        }
        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task UserDelete(string id)
        {
            ApplicationUser user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
        public async Task UpdateUser(string id, string email)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            user.Email = email;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdatePassword(string id, string password)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user,password);
        }


    }
}
