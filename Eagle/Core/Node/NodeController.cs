using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.EntityFrameworkCore;
using Core.DbTables;
using DomainModels;
using Utility;

namespace Core.Node
{
    public class NodeController : CoreController
    {
        // GET: api/Nodes
        [HttpGet("{bundleId}")]
        public IEnumerable<Object> GetNodes([FromRoute] string bundleId)
        {
            var query = from node in dc.Nodes
                        join bundle in dc.Bundles on node.BundleId equals bundle.Id
                        where bundle.Id == bundleId
                        select new
                        {
                            Id = node.Id,
                            Name = node.Name,
                            Description = node.Description,
                            EntityName = bundle.EntityName,
                            BundleId = bundle.Id,
                            Status = node.Status.ToString()
                        };

            return query.ToList();
        }

        // GET: api/Node/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNode([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = await dc.Nodes.SingleOrDefaultAsync(m => m.Id == id);

            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }

        // POST: api/Node
        [HttpPost]
        public async Task<IActionResult> PostNode([FromBody] DmNode node)
        {
            node.CreatedUserId = GetCurrentUser().Id;
            node.CreatedTime = DateTime.UtcNow;
            node.ModifiedUserId = GetCurrentUser().Id;
            node.ModifiedTime = DateTime.UtcNow;

            dc.Transaction(delegate
            {
                NodeEntity nodeRecord = node.Map<NodeEntity>();
                dc.Nodes.Add(nodeRecord);

                dc.SaveChanges();

                node.FieldRecords
                    .Where(x => x.FieldTypeId == Enums.FieldTypes.Text)
                    .ToList()
                    .ForEach(fieldRecord =>
                    {
                        fieldRecord.Data.ForEach(data => {
                            var record = data.ToObject<NodeTextFieldEntity>();
                            record.EntityId = nodeRecord.Id;
                            record.BundleFieldId = fieldRecord.BundleFieldId;
                            dc.NodeTextFields.Add(record);
                        });
                    });

                node.FieldRecords
                    .Where(x => x.FieldTypeId == Enums.FieldTypes.Address)
                    .ToList()
                    .ForEach(fieldRecord =>
                    {
                        fieldRecord.Data.ForEach(data => {
                            var record = data.ToObject<NodeAddressFieldEntity>();
                            record.EntityId = nodeRecord.Id;
                            record.BundleFieldId = fieldRecord.BundleFieldId;
                            dc.NodeAddressFields.Add(record);
                        });
                    });
            });

            return CreatedAtAction("GetNode", new { id = node.Id }, node);
        }
    }
}