using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eagle.Enums;
using Eagle.Utility;
using Eagle.Models;

namespace Eagle.Core.Field
{
    public class FieldController : CoreController
    {
        [HttpGet("Types")]
        public IEnumerable<Object> GetTypes()
        {
            /*List<String> fieldTypeNames = new List<string>();

            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            List<Type> fields = types.Where(t => t.GetInterfaces().Contains(typeof(IField))).ToList();
            fields.ForEach(type => {
                fieldTypeNames.Add(type.Name.Replace("Field",""));
            });

            return fieldTypeNames.OrderBy(x => x);*/

            return EnumHelper.GetValues<FieldTypes>().Select(x => new { FieldTypeId = x, FieldTypeName = x.ToString() }).OrderBy(x => x.FieldTypeName);
        }
    }
}
