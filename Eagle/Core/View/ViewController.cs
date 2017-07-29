using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels;
using Core.Enums;
using Core.Account;
using Core.Block;

namespace Core.View
{
    public class ViewController : CoreController
    {
        // GET: api/Nodes
        [HttpGet("{viewId}/execute")]
        public ViewEntity Execute([FromRoute] string viewId)
        {
            var dm = new DomainModel<ViewEntity>(dc, new ViewEntity { Id = viewId });
            dm.Load();

            dm.Entity.Data.AddRange(dc.Table<UserEntity>().ToList());

            return dm.Entity;

                // Fake data
                /*dmView = new ViewEntity()
                {
                    Id = "00001-1211dd-abcc",
                    Name = "Invoice Batches",
                    RepresentType = RepresentType.Grid,
                };

                dmView.Columns.AddRange(new List<ViewColumEntity> {
                    new ViewColumEntity { DisplayName = "Batch Invoice Num", FieldName = "batchInvoiceNum", FieldType = FieldTypes.Text }
                });

                List<Object> invoiceBatches = new List<object>();
                invoiceBatches.Add(new { BatchInvoiceNum = "FM CAH 12.8.2015" });
                invoiceBatches.Add(new { BatchInvoiceNum = "FM CAH 1.16.2015" });
                invoiceBatches.Add(new { BatchInvoiceNum = "FM COMP 2.24.2016" });
                invoiceBatches.Add(new { BatchInvoiceNum = "RMS ORAPJ 4.5.2016" });
                invoiceBatches.Add(new { BatchInvoiceNum = "FM BGR 12.4.2015" });
                invoiceBatches.Add(new { BatchInvoiceNum = "FM BGR 11.20.2015" });
                invoiceBatches.Add(new { BatchInvoiceNum = "FM TMUS 11.11.2015" });
                invoiceBatches.Add(new { BatchInvoiceNum = "RMS DEXJ 2.16.2016" });

                dmView.Data.AddRange(invoiceBatches);

                dmView.Actions.AddRange(
                    new List<ViewActionEntity> {
                    new ViewActionEntity { Name = "Bridge AP", RequestUrl = "/api/Account", RequestMethod = "POST" },
                    new ViewActionEntity { Name = "Bridge AR", RequestUrl = "/api/Account", RequestMethod = "POST" }
                    }
                );*/

            

        }
    }
}
