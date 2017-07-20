using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Bundle;
using Core.Interfaces;

namespace Core.DomainModels
{
    public class DmTaxonomy : IDomainModel, IBundlable
    {
        public DmTaxonomy()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BundleId { get; set; }

        public void LoadFieldRecords(CoreDbContext dc)
        {
            throw new NotImplementedException();
        }
    }
}
