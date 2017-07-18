using Eagle.Core.Interfaces;
using Eagle.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Core.Account
{
    public class DmAccount : IDomainModel
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

        public string ModifiedUserId { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
