using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Models;

namespace Core.Field
{
    public class TaxonomyTermField : IFieldModel
    {
        public IEnumerable<string> FieldConfigNames()
        {
            return new List<String>() { "TaxonomyId" };
        }
    }

    public abstract class TaxonomyTermFieldEntity : FieldRepositoryEntity
    {
        public int TaxonomyTermId { get; set; }
        public TaxonomyTermEntity TaxonomyTerm { get; set; }
    }
}
