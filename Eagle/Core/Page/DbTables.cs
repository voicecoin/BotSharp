using Core.DbTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Page
{
    public class PageEntity : DbRecordWithName
    {
        public String Description { get; set; }
    }
}
