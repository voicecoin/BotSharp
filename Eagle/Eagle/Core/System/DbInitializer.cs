using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Eagle.Core.Interfaces;
using Eagle.DataContexts;
using Eagle.Utility;

namespace Eagle.Core
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            CoreDbContext dc = new CoreDbContext(new DbContextOptions<CoreDbContext>());
            dc.CurrentUser = new Account.DmAccount { Id = Constants.SystemUserId };
            dc.Database.EnsureCreated();

            var instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>();

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction(delegate
                    {
                        instance.Load(env, dc);
                    });
                });
        }
    }
}
