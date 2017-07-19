using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Models;

namespace Core.Field
{
    public class DmTextField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }

    public abstract class TextFieldEntity : FieldRepositoryEntity
    {
        [DataType(DataType.Text)]
        public String Value { get; set; }
    }
}
