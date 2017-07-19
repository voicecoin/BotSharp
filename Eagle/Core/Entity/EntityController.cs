using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Bundle;

namespace Core.Entity
{
    public class EntityController : CoreController
    {
        [HttpGet("Bundlables")]
        public IEnumerable<String> GetBundlableEntities()
        {
            List<String> bundlableEntities = new List<string>();

            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            List<Type> fields = types.Where(t => t.GetInterfaces().Contains(typeof(IBundlable))).ToList();
            fields.ForEach(type => {
                bundlableEntities.Add(type.Name.Replace("Entity", ""));
            });

            return bundlableEntities.OrderBy(x => x);
        }
    }
}
