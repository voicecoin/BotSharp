using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.Core.Field
{
    public class DateTimeField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }
    public abstract class DateTimeFieldEntity : FieldRepositoryEntity
    {
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }
    }
}
