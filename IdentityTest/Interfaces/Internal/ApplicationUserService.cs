using IdentityTest.Models;
using IdentityTest.Web.Interfaces;
using IdetityTest.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IdentityTest.Web.Controllers;
using System.Runtime.InteropServices;
using IdentityTest.Web.ViewModels;
using System.Security.AccessControl;
using System.Security.Claims;

namespace IdentityTest.Web
{
    internal class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public ApplicationUserService(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager)
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

        private IEnumerable<Claim> UserPersonalInfo(string firstName,string lastName, string address)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("FirstName", firstName));
            claims.Add(new Claim("LastName", lastName));
            claims.Add(new Claim("Address", address));

            return claims;
        }

        public async Task RegisterUserAsync(UserDTO user)
        {
            ApplicationUser newUser = new() {
                Email = user.Email,
                EmailConfirmed = true,
                UserName = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            IEnumerable<Claim> claim = UserPersonalInfo(user.FirstName, user.LastName, user.Address);

            if (user.Password.Equals(user.PasswordConfirm))
            {
                await _userManager.CreateAsync(newUser, user.Password);
                await _userManager.AddClaimsAsync(newUser, claim);
            }
            
            

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
        public async Task UpdateUser(string id, UserUpdateViewModel model)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            user.Email = model.UserEmail;
            user.UserName = model.UserEmail;
            user.PhoneNumber = model.UserPhoneNumber;

            var claims = await _userManager.GetClaimsAsync(user);

            var OldFirstName = claims.Single(x => x.Type.Equals("FirstName"));
            var OldLastName = claims.Single(x => x.Type.Equals("LastName"));
            var OldAddress = claims.Single(x => x.Type.Equals("Address"));

            Claim FirstName = new ("FirstName",model.UserFirstName);
            Claim LastName = new("LastName", model.UserLastName);
            Claim Address= new("FirstName", model.UserAddress);

            if (!OldFirstName.Value.Equals(FirstName.Value))
            {
                await _userManager.ReplaceClaimAsync(user,OldFirstName,FirstName);
            }
            if (!OldLastName.Value.Equals(LastName.Value))
            {
                await _userManager.ReplaceClaimAsync(user, OldLastName, LastName);
            }
            if (!OldAddress.Value.Equals(Address.Value))
            {
                await _userManager.ReplaceClaimAsync(user, OldAddress, Address);
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdatePassword(string id, string password)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user,password);
        }

        public async Task<UserUpdateViewModel> getUserToUpdate(string Id)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(Id));
            var claims = await _userManager.GetClaimsAsync(user);
            var FirstName = claims.Single(x => x.Type.Equals("FirstName"));
            var LastName = claims.Single(x => x.Type.Equals("LastName"));
            var Address = claims.Single(x => x.Type.Equals("Address"));

            UserUpdateViewModel toUpdate = new();
            toUpdate.UserId = Id;
            toUpdate.UserEmail = user.Email;
            toUpdate.UserFirstName = FirstName.Value;
            toUpdate.UserLastName = LastName.Value;
            toUpdate.UserAddress = Address.Value;
            toUpdate.UserPhoneNumber = user.PhoneNumber;

            return toUpdate;
        }
    }
}
