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
using NuGet.Protocol.Core.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.VisualBasic;

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
        public async Task<UserIndexViewModel> GetFromId(string id)
        {
            UserIndexViewModel model = new();
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));

            model.UserId = user.Id;
            model.UserEmail = user.Email;
            model.UserPhoneNumber = user.PhoneNumber;


            var claims = await _userManager.GetClaimsAsync(user);

            
            var FirstName = claims.FirstOrDefault(x => x.Type.Equals("FirstName"));
            if (FirstName == null)
            {
                model.UserFirstName = "Add the FirstName!";
            }
            else
            {
                model.UserFirstName = FirstName.Value;
            }
            var LastName = claims.FirstOrDefault(x => x.Type.Equals("LastName"));
            if (LastName == null)
            {
                model.UserLastName = "Add the LastName!";
            }
            else
            {
                model.UserLastName = LastName.Value;
            }
            var Address = claims.FirstOrDefault(x => x.Type.Equals("Address"));
            if (Address == null)
            {
                model.UserAddress = "Add the Addres!";
            }
            else
            {
                model.UserAddress = Address.Value;
            }
           
            model.UserRoles = await _userManager.GetRolesAsync(user);

            return model;
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


            var OldFirstName = claims.FirstOrDefault(x => x.Type.Equals("FirstName"));
            var OldLastName = claims.FirstOrDefault(x => x.Type.Equals("LastName"));
            var OldAddress = claims.FirstOrDefault(x => x.Type.Equals("Address"));

            if (OldFirstName == null)
            {
                Claim FirstName = new("FirstName", model.UserFirstName);
                await _userManager.AddClaimAsync(user, FirstName);
            }
            else if (OldFirstName.Value.Equals(model.UserFirstName))
            {
                Claim FirstName = new("FirstName", model.UserFirstName);
                await _userManager.ReplaceClaimAsync(user, OldFirstName, FirstName);
            }

            if(OldLastName == null)
            {
                Claim LastName = new("LastName", model.UserLastName);
                await _userManager.AddClaimAsync(user,LastName);
            }
            else if (OldLastName.Value.Equals(model.UserLastName))
            {
                Claim LastName = new("LastName", model.UserLastName);
                await _userManager.ReplaceClaimAsync(user, OldLastName, LastName);
            }

            if (OldAddress==null)
            {
                Claim Address = new("Address", model.UserAddress);
                await _userManager.AddClaimAsync(user, Address);
            }
            else if(OldAddress.Value.Equals(model.UserAddress))
            {
                Claim Address = new("Address", model.UserAddress);
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

            UserUpdateViewModel toUpdate = new();

            var FirstName = claims.FirstOrDefault(x => x.Type.Equals("FirstName"));
            if (FirstName == null)
            {
                toUpdate.UserFirstName = "Add the FirstName!";
            }
            else
            {
                toUpdate.UserFirstName = FirstName.Value;
            }
            var LastName = claims.FirstOrDefault(x => x.Type.Equals("LastName"));
            if (LastName == null)
            {
                toUpdate.UserLastName =  "Add the LastName!";
            }
            else
            {
                toUpdate.UserLastName = LastName.Value;
            }
            var Address = claims.FirstOrDefault(x => x.Type.Equals("Address"));
            if (Address == null)
            {
                toUpdate.UserAddress = "Add the Addres!";
            }
            else
            {
                toUpdate.UserAddress = Address.Value;
            }


            toUpdate.UserId = Id;
            toUpdate.UserEmail = user.Email;
            
            
            
            toUpdate.UserPhoneNumber = user.PhoneNumber;

            return toUpdate;
        }

        public async Task<ChangePasswordViewModel> GetChangePassword(string Id)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(Id));
            ChangePasswordViewModel newPassword =  new();
            newPassword.Email = user.Email;
            newPassword.UserId = user.Id;

            return newPassword;
        }

        public async Task<ConfirmAccountDelete> ToBeDeleted(string id)
        {
            var user = await _userManager.Users.SingleAsync(x => x.Id.Equals(id));
            
            ConfirmAccountDelete confirmAccountDelete = new();

            confirmAccountDelete.UserId = user.Id;
            confirmAccountDelete.Email = user.Email;

            return confirmAccountDelete;
        }
    }
}
