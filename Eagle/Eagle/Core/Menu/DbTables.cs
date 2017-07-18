using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core;
using Eagle.DataContexts;

namespace Eagle.DbTables
{
    public class MenuEntity : DbRecordWithNameColumn
    {
        [MaxLength(64)]
        public string Description { get; set; }
        [MaxLength(16)]
        public string Icon { get; set; }
        [MaxLength(64)]
        public String Link { get; set; }
    }
}
