using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Bundle;
using Core.Interfaces;

namespace Core.Account
{
    public class DmAccount : IDomainModel, IBundlable
    {
        public DmAccount()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string BundleId { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public String Description { get; set; }

        public void LoadFieldRecords(CoreDbContext dc)
        {
            
        }
    }
}
