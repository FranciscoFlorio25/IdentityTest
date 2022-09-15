using IdentityTest.Application.Data;
using IdetityTest.Infraestrtucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdetityTest.Infraestrtucture
{
    public static class DependencyInyection
    {
        public static IServiceCollection MyIdentityTestInfraestructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<IIdentityContext, IdentityContext>(o => 
            o.UseSqlServer(configuration.GetConnectionString("Default")));

            return service;
        }
    }
}
