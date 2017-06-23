using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    public class Intents : DbTable
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        [MaxLength(32)]
        public String Name { get; set; }
    }

    public class IntentInputContexts: DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
    }

    public class IntentOutputContexts : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        public int? Lifespan { get; set; }
    }

    public class IntentExpressions : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
    }

    public class IntentExpressionItems : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentExpressionId { get; set; }
        [Required]
        [StringLength(64)]
        public String Text { get; set; }
        [StringLength(36)]
        public String EntityId { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
    }

    public class IntentEvents : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
    }

    public class IntentActions : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
    }
}
