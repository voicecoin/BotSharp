using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Account
{
    public class HookDbInitializer : IHookDbInitializer
    {
        public int Priority => 100;

        public void Load(Database dc)
        {
            InitRoles(dc);
            InitUsers(dc);
        }

        private void InitRoles(Database dc)
        {
            if (dc.Table<Role>().Any()) return;

            dc.Table<Role>().Add(new Role
            {
                Id = "092c5f0f-1028-419d-82f3-534a31d391e3",
                Name = "Authenticated User",
                Description = "Authenticated User"
            });
        }

        private void InitUsers(Database dc)
        {
            dc.Table<User>().Add(new User
            {

            });
        }
    }

}
