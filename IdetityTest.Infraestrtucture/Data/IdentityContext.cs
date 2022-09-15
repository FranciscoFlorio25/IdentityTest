using IdentityTest.Application.Data;
using IdentityTest.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdetityTest.Infraestrtucture.Data
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>, IIdentityContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
