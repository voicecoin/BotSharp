using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Bundle;
using Core.Interfaces;

namespace Core.Registry
{
    public class DmRegistry : IDomainModel
    {
        public DmRegistry()
        {
            Id = Guid.NewGuid().ToString();
        }

        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }

    public class DmRegistryEntry : IDomainModel
    {
        public DmRegistryEntry()
        {

        }

        public String Id { get; set; }
        public String RegistryId { get; set; }
        public String Name { get; set; }
        public Object Value { get; set; }
    }

}
