using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core;
using Core.Field;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Enums;
using Core.Interfaces;
using Core.Bundle;
using Core.DataContexts;
using Core.DbTables;

namespace DomainModels
{
    public class DmNode : IDomainModel, IBundlable
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

        public IQueryable<NodeTextFieldEntity> LoadTextFields(CoreDbContext dc)
        {
            var query = from bundleField in dc.BundleFields
                        join textField in dc.NodeTextFields on bundleField.Id equals textField.BundleFieldId
                        where bundleField.BundleId == BundleId
                            && bundleField.FieldTypeId.Equals(FieldTypes.Text)
                            && textField.EntityId == Id
                        select textField;

            string sql = query.ToString();

            return query;
        }

        public void LoadFieldRecords(CoreDbContext dc)
        {
            var FieldRecords = new List<object>();
            dc.BundleFields.Where(x => x.BundleId == BundleId).ToList().ForEach(field =>
            {
                switch (field.FieldTypeId)
                {
                    case FieldTypes.Text:
                        FieldRecords.AddRange(dc.NodeTextFields.Where(x => x.BundleFieldId == field.Id));
                        break;
                }
            });
        }
    }

    public class DmFieldRecord
    {
        public FieldTypes FieldTypeId { get; set; }
        public string BundleFieldId { get; set; }
        public List<JObject> Data { get; set; }
    }
}
