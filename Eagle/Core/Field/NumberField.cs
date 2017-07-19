using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Models;

namespace Core.Field
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
