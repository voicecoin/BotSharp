using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Field
{
    public class RichTextField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }

    public abstract class RichTextFieldEntity : FieldRepositoryEntity
    {
        [DataType(DataType.Text)]
        public String Data { get; set; }
    }
}
