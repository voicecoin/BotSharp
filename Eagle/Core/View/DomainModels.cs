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
using Core.View;
using Core.Enums;

namespace DomainModels
{
    public class DmView
    {
        public DmView()
        {
            Data = new List<object>();
            Columns = new List<DmViewHeader>();
            Actions = new List<DmViewAction> { };
        }

        public String Id { get; set; }
        /// <summary>
        /// For table view as key
        /// </summary>
        public String Key { get; set; }
        public String Name { get; set; }
        public RepresentType RepresentType { get; set; }
        public List<Object> Data { get; set; }
        public List<DmViewHeader> Columns { get; set; }
        public List<DmViewAction> Actions { get; set; }
    }

    public class DmViewHeader
    {
        public String DisplayName { get; set; }
        public String FieldName { get; set; }
        public FieldTypes FieldType { get; set; }
    }

    public class DmViewAction
    {
        public String Name { get; set; }
        public String RequestUrl { get; set; }
        public String RequestMethod { get; set; }
        public String RedirectUrl { get; set; }
    }
}
