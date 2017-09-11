using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Bundle;
using Utility;

namespace Core.Entity
{
    public class EntityController : CoreController
    {
        [HttpGet("Bundlables")]
        public IEnumerable<String> GetBundlableEntities()
        {
            List<String> bundlableEntities = new List<string>();

            List<Type> core = TypeHelper.GetClassesWithInterface(typeof(IBundlable), "Core");
            core.ForEach(type => {
                bundlableEntities.Add(type.Name.Replace("Entity", ""));
            });

            List<Type> app = TypeHelper.GetClassesWithInterface(typeof(IBundlable), "Apps");
            app.ForEach(type => {
                bundlableEntities.Add(type.Name.Replace("Entity", ""));
            });

            return bundlableEntities.OrderBy(x => x);
        }
    }
}
