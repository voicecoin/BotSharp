using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Core.Enums;

namespace Core.DomainModels
{
    public class DmBundle : IDomainModel
    {
        public DmBundle()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string EntityName { get; set; }
    }

    public class DmBundleField : IDomainModel
    {
        public DmBundleField()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public FieldTypes FieldTypeId { get; set; }
        public string BundleId { get; set; }
    }
}
