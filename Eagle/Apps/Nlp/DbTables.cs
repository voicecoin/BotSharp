using Core;
using Core.Interfaces;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Apps.Nlp
{
    [Table("NlpParseCache")]
    public class NlpParseCache : IDbRecord
    {
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }
        public NlpEngine Parser { get; set; }
        public String ParsedJson { get; set; }
        public CacheType Type { get; set; }
        public String Text { get; set; }
    }
}
