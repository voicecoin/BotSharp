﻿using Core;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.Intent
{
    [Table("Chatbot_Intents")]
    public class IntentEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        [MaxLength(32)]
        public String Name { get; set; }
        [MaxLength(256)]
        public String Description { get; set; }

        [JsonIgnore]
        [Column("Contexts")]
        [MaxLength(256)]
        public String ContextsJson { get; set; }
        [NotMapped]
        public List<String> Contexts { get; set; }

        [MaxLength(16)]
        public String Keyword { get; set; }

        [JsonIgnore]
        [Column("Events")]
        [MaxLength(128)]
        public String EventsJson { get; set; }
        [NotMapped]
        public List<String> Events { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<IntentEntity>().Any(x => x.AgentId == AgentId && x.Name == Name);
        }

        [NotMapped]
        public List<String> Templates { get; set; }
        [NotMapped]
        public List<IntentExpressionEntity> UserSays { get; set; }
        [NotMapped]
        public List<IntentResponseEntity> Responses { get; set; }
    }

    [Table("Chatbot_IntentExpressions")]
    public class IntentExpressionEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }
        [Required]
        [MaxLength(128)]
        public String Text { get; set; }

        [NotMapped]
        public Int32 Count { get; set; }
        [NotMapped]
        public double Similarity { get; set; }

        [JsonIgnore]
        [Column("Data")]
        [MaxLength]
        public String DataJson { get; set; }
        [NotMapped]
        public List<DmIntentExpressionItem> Data { get; set; }

        /*[JsonIgnore]
        [Column("Dependency")]
        [MaxLength]
        public String DependencyJson { get; set; }
        [NotMapped]
        public List<DmIntentExpressionItem> Dependency { get; set; }*/

        public Boolean AllowOverrideData { get; set; }

        [NotMapped]
        public bool IsFaq { get; set; }
        [NotMapped]
        public String FaqAnswer { get; set; }

        /// <summary>
        /// for frontend flag
        /// </summary>
        [NotMapped]
        public Boolean Visible { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<IntentExpressionEntity>().Any(x => x.IntentId == IntentId && x.Text == Text);
        }
    }

    [Table("Chatbot_IntentResponses")]
    public class IntentResponseEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public String IntentId { get; set; }

        [MaxLength(128)]
        public String Action { get; set; }

        [NotMapped]
        public List<IntentResponseMessageEntity> Messages { get; set; }
        [NotMapped]
        public List<IntentResponseParameterEntity> Parameters { get; set; }

        [JsonIgnore]
        [Column("AffectedContexts")]
        public String AffectedContextsJson { get; set; }
        [NotMapped]
        public List<DmIntentResponseContext> AffectedContexts { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<IntentResponseEntity>().Any(x => x.IntentId == IntentId && x.AffectedContextsJson == AffectedContextsJson);
        }
    }

    [Table("Chatbot_IntentResponseMessages")]
    public class IntentResponseMessageEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseId { get; set; }
        public IntentResponseMessageType Type { get; set; }
        public IntentResponseMessagePlatform Platform { get; set; }

        [JsonIgnore]
        [Column("Speeches")]
        public String SpeechesJson { get; set; }
        [NotMapped]
        public List<String> Speeches { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<IntentResponseMessageEntity>().Any(x => x.IntentResponseId == IntentResponseId && x.SpeechesJson == SpeechesJson);
        }
    }

    [Table("Chatbot_IntentResponseParameters")]
    public class IntentResponseParameterEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public String IntentResponseId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        public Boolean IsList { get; set; }
        /// <summary>
        /// Entity 
        /// </summary>
        [StringLength(36)]
        public String DataType { get; set; }
        public Boolean Required { get; set; }
        [MaxLength(32)]
        public String Value { get; set; }
        [MaxLength(64)]
        public String DefaultValue { get; set; }

        [JsonIgnore]
        [Column("Prompts")]
        public String PromptsJson { get; set; }
        [NotMapped]
        public List<String> Prompts { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<IntentResponseParameterEntity>().Any(x => x.IntentResponseId == IntentResponseId && x.PromptsJson == PromptsJson);
        }
    }
}
