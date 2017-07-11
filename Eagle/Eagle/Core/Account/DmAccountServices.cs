using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DmServices
{
    public static class DmAccountServices
    {
        public static void Create(this DmAccount agentRequestModel, CoreDbContext dc)
        {
            var user = dc.Users.Find(agentRequestModel.Id);
            if (user != null) return;

            var accountRecord = agentRequestModel.Map<UserEntity>();
            agentRequestModel.CreatedDate = DateTime.UtcNow;
            agentRequestModel.ModifiedDate = DateTime.UtcNow;

            dc.Users.Add(accountRecord);
        }
    }
}
