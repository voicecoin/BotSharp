using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Enums;
using Core.Account;
using Core.Block;
using Utility;

namespace Core.View
{
    public class ViewController : CoreController
    {
        [HttpGet("{id}")]
        public ViewEntity GetView(string id)
        {
            var dm = new DomainModel<ViewEntity>(dc, new ViewEntity { Id = id });
            dm.LoadDefinition();

            return dm.Entity;
        }

        [HttpGet("Query")]
        public DmPageResult<ViewEntity> GetViews(string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<ViewEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            return new DmPageResult<ViewEntity>() { Page = page }.LoadDataByPage(query);
        }

        // GET: api/Nodes
        [HttpGet("{viewId}/execute")]
        public async Task<ViewEntity> Execute([FromRoute] string viewId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var dm = new DomainModel<ViewEntity>(dc, new ViewEntity { Id = viewId });
            dm.Load();
            dm.Entity.Result = new DmPageResult<object> { Page = page, Size = size, Items = new List<object>() };

            if (dm.Entity.DataContainer == DataContainer.SeflHost)
            {
                dm.Entity.Result.LoadDataByPage(dc.Table<UserEntity>());
            }
            else if (dm.Entity.DataContainer == DataContainer.RestApi)
            {
                string host = GetConfig(dm.Entity.RestApiHost);
                var items = await HttpClientHelper.GetAsJsonAsync<IEnumerable<JObject>>(host, dm.Entity.RestApiPath);
                dm.Entity.Result.Total = items.Count();
                dm.Entity.Result.Page = 1;
                dm.Entity.Result.Size = items.Count();
                dm.Entity.Result.Items.AddRange(items.Skip((page-1) * size).Take(size));
            }

            return dm.Entity;
        }
    }
}
