using EntityFrameworkCore.BootKit;
using System;
using System.ComponentModel.DataAnnotations;

namespace Voicebot.Core.Account
{
    public class RoleOfUser : DbRecord, IDbRecord
    {
        [StringLength(36)]
        public String UserId { get; set; }

        public User User { get; set; }

        [StringLength(36)]
        public String RoleId { get; set; }

        public Role Role { get; set; }
    }
}
