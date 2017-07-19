using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Core.DataContexts;
using Core.Bundle;

namespace Core.DbTables
{
    public class TaxonomyEntity : DbRecord
    {
        [Required]
        public string BundleId { get; set; }

        [MaxLength(512, ErrorMessage = "Description cannot be longer than 512 characters.")]
        public String Description { get; set; }
    }

    public class TaxonomyTermEntity : DbRecordWithName
    {
        [Required]
        public string TaxonomyId { get; set; }
        public TaxonomyEntity Taxonomy { get; set; }

        /// <summary>
        /// Parent Term, In order to create taxonomy with hierarchy
        /// </summary>
        public string ParentId { get; set; }
    }
}
