using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Field
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
