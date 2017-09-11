using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Core.Interfaces;
using Core.Bundle;

namespace Core.Account
{
    public static class DmAccountService
    {
        public static bool Add(this BundleDomainModel<UserEntity> userModel, CoreDbContext dc)
        {
            return userModel.AddEntity();
        }

    }
}
