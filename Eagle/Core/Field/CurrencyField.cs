using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Field
{
    public class CurrencyField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }
    public abstract class CurrencyFieldEntity : FieldRepositoryEntity
    {
        [DataType(DataType.Currency)]
        public decimal Data { get; set; }
    }
}
