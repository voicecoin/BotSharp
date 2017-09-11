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
using System.Net;
using System.Text;

namespace Core.Block
{
    [Table("Views")]
    public class ViewEntity : DbRecord, IDbRecord4Core
    {
        public String Name { get; set; }
        public RepresentType RepresentType { get; set; }
        [NotMapped]
        public DmPageResult<Object> Result { get; set; }
        [NotMapped]
        public List<ViewColumEntity> Columns { get; set; }
        [NotMapped]
        public List<ViewActionEntity> Actions { get; set; }

        /// <summary>
        /// 有些数据Host在外部，通过Rest Api去取数据
        /// </summary>
        public DataContainer DataContainer { get; set; }
        /// <summary>
        /// Only valide when DataContainer is RestApi, config name such config.GetSection("SiteSetting:WebApi").Value
        /// </summary>
        public String RestApiHost { get; set; }
        /// <summary>
        /// Only valide when DataContainer is RestApi
        /// </summary>
        public String RestApiPath { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewEntity>().Any(x => x.Name == Name);
        }
    }

    [Table("ViewColumns")]
    public class ViewColumEntity : DbRecord, IDbRecord4Core
    {
        [JsonIgnore]
        [StringLength(36)]
        public String ViewId { get; set; }
        public String DisplayName { get; set; }
        public Boolean HideName { get; set; }
        public String FieldName { get; set; }
        public FieldTypes FieldType { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewColumEntity>().Any(x => x.ViewId == ViewId && x.FieldName == FieldName);
        }
    }

    [Table("ViewActions")]
    public class ViewActionEntity : DbRecord, IDbRecord4Core
    {
        [JsonIgnore]
        [StringLength(36)]
        public String ViewId { get; set; }
        public String Name { get; set; }
        public String Icon { get; set; }
        /// <summary>
        /// Actions show in data line lelve or view level
        /// </summary>
        public Boolean IsViewLevel { get; set; }
        [JsonIgnore]
        public String RestApiHost { get; set; }
        [JsonIgnore]
        public String RestApiPath { get; set; }
        [NotMapped]
        public String RequestUrl { get { return String.IsNullOrEmpty(RestApiPath) ? "" : $"/http?host={WebUtility.UrlEncode(RestApiHost)}&path={WebUtility.UrlEncode(RestApiPath)}"; } }
        public String RequestMethod { get; set; }
        public String RedirectUrl { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<ViewActionEntity>().Any(x => x.ViewId == ViewId && x.Name == Name);
        }
    }
}
