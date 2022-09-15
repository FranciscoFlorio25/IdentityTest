using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTest.Application.Data
{
    public interface IIdentityContext
    {

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
