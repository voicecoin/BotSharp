using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.Core.Field
{
    public class NumberField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }

    public abstract class NumberFieldEntity : FieldRepositoryEntity
    {
        public decimal Data { get; set; }
    }
}
