using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.Core.Field
{
    public class ImageField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>();
        }
    }

    public abstract class ImageFieldEntity : FieldRepositoryEntity
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }
    }
}
