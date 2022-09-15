using IdentityTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdetityTest.Web.Data
{
    public class CiscoShopContext : IdentityDbContext<ApplicationUser>
    {
        public CiscoShopContext(DbContextOptions<CiscoShopContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
