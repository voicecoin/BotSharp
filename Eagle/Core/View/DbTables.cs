using Core.Block;
using Core.Enums;
using Core.Interfaces;
using Core.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Core.Block
{
    [Table("Views")]
    public class ViewEntity : DbRecord, IDbRecord4SqlServer
    {
        /// <summary>
        /// For table view as key
        /// </summary>
        [NotMapped]
        public String Key { get; set; }
        public String Name { get; set; }
        public RepresentType RepresentType { get; set; }
        [NotMapped]
        public List<Object> Data { get; set; }
        [NotMapped]
        public List<ViewColumEntity> Columns { get; set; }
        [NotMapped]
        public List<ViewActionEntity> Actions { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewEntity>().Any(x => x.Name == Name);
        }
    }

    [Table("ViewColumns")]
    public class ViewColumEntity : DbRecord, IDbRecord4SqlServer
    {
        [JsonIgnore]
        [StringLength(36)]
        public String ViewId { get; set; }
        public String DisplayName { get; set; }
        public String FieldName { get; set; }
        public FieldTypes FieldType { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewColumEntity>().Any(x => x.ViewId == ViewId && x.FieldName == FieldName);
        }
    }

    [Table("ViewActions")]
    public class ViewActionEntity : DbRecord, IDbRecord4SqlServer
    {
        [JsonIgnore]
        [StringLength(36)]
        public String ViewId { get; set; }
        public String Name { get; set; }
        public String RequestUrl { get; set; }
        public String RequestMethod { get; set; }
        public String RedirectUrl { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewActionEntity>().Any(x => x.ViewId == ViewId && x.Name == Name);
        }
    }
}
