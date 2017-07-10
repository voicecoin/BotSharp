using Eagle.Enums;
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

    public class IntentExpressions : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(128)]
        public String Text { get; set; }
        [Required]
        [MaxLength(128)]
        public String Template { get; set; }
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
        [Required]
        public int Position { get; set; }
        [Required]
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

    public class IntentResponses : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [MaxLength(128)]
        public String Action { get; set; }
    }

    public class IntentResponseContexts : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        public int? Lifespan { get; set; }
    }

    public class IntentResponseMessages : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseId { get; set; }
        public IntentResponseMessageType Type { get; set; }
        public IntentResponseMessagePlatform Platform { get; set; }
    }

    public class IntentResponseMessageContents : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseMessageId { get; set; }
        [Required]
        [MaxLength(512)]
        public String Content { get; set; }
    }

    public class IntentResponseParameters : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        public Boolean IsList { get; set; }
        /// <summary>
        /// Entity Id
        /// </summary>
        [StringLength(36)]
        public String EntityId { get; set; }
        public Boolean Required { get; set; }
        [MaxLength(32)]
        public String Value { get; set; }
        [MaxLength(64)]
        public String DefaultValue { get; set; }
    }

    public class IntentResponseParameterPrompts : DbTable
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseParameterId { get; set; }
        [MaxLength(64)]
        public String Text { get; set; }
    }
}
