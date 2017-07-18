using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Eagle.Core;
using Eagle.Core.Field;
using Eagle.DataContexts;
using Microsoft.EntityFrameworkCore;
using Eagle.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eagle.DomainModels
{
    public class DmNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BundleId { get; set; }
        public EntityStatus Status { get; set; }
        public List<DmFieldRecord> FieldRecords { get; set; }

        public DateTime CreatedTime { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ModifiedUserId { get; set; }
    }

    public class DmFieldRecord
    {
        public FieldTypes FieldTypeId { get; set; }
        public string BundleFieldId { get; set; }
        public List<JObject> Data { get; set; }
    }
}
