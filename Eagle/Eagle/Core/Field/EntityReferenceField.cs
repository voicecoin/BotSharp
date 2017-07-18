using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.Core.Field
{
    public class EntityReferenceField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>() { "DisplayName",  "BundleName" };
        }
    }

    public abstract class EntityReferenceFieldEntity : FieldRepositoryEntity
    {
        public decimal Data { get; set; }
    }
}
