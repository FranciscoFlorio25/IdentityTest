
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTest.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string firstname, string lastname, string password, string email)
        {
            Firstname = firstname;
            Lastname = lastname;
            this.password = password;
            Email = email;
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string password { get; set; }
        public string Email { get; set; }
    }
}
