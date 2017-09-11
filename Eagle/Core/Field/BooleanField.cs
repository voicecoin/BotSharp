using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Field
{
    public class DmBooleanField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }

    public abstract class BooleanFieldEntity : FieldRepositoryEntity
    {
        [DataType(DataType.Custom)]
        public bool Value { get; set; }
    }
}
