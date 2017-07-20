using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels;
using Core.Enums;
using Core.Account;

namespace Core.View
{
    public class ViewController : CoreController
    {
        // GET: api/Nodes
        [HttpGet("{viewId}/execute")]
        public IEnumerable<DmView> Execute([FromRoute] string viewId)
        {
            DmView dmView = new DmView() {
                Id = "00001-123-abcc",
                Name = "Users",
                RepresentType = RepresentType.Table,
            };

            dmView.Columns.AddRange(new List<DmViewHeader> {
                new DmViewHeader { DisplayName = "Avatar", FieldName = "avatar", FieldType = FieldTypes.Image },
                new DmViewHeader { DisplayName = "User Name", FieldName = "userName", FieldType = FieldTypes.Text },
                new DmViewHeader { DisplayName = "First Name", FieldName = "firstName", FieldType = FieldTypes.Text },
                new DmViewHeader { DisplayName = "Last Name", FieldName = "lastName", FieldType = FieldTypes.Text },
                new DmViewHeader { DisplayName = "Email", FieldName = "email", FieldType = FieldTypes.Text },
                new DmViewHeader { DisplayName = "Created Date", FieldName = "createdDate", FieldType = FieldTypes.DateTime },
                new DmViewHeader { DisplayName = "Status", FieldName = "status", FieldType = FieldTypes.Boolean }
            });

            dmView.Data.AddRange(dc.Table<UserEntity>().ToList());

            dmView.Actions.AddRange(
                new List<DmViewAction> {
                    new DmViewAction { Name = "Edit", RedirectUrl = "/Configuration/People/Profile" },
                    new DmViewAction { Name = "Delete", RequestUrl = "/api/Account", RequestMethod = "DELETE" }
                }
            );

            return new List<DmView> { dmView };
        }
    }
}
