using Apps.Chatbot.Agent;
using Apps.Chatbot.Entity;
using Apps.Chatbot.Intent;
using Core;
using Core.Account;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;

namespace Apps.Baas
{
    public class DashboardController : CoreController
    {
        [HttpGet("Statistics")]
        public object Statistics()
        {
            return new
            {
                UserTotal = dc.Table<UserEntity>().Count(),
                AgentTotal = dc.Table<AgentEntity>().Count(),
                EntityTotal = dc.Table<EntityEntity>().Count(),
                IntentTotal = dc.Table<IntentEntity>().Count(),
                UserWordTotal = dc.Table<EntityEntryEntity>().Count()
            };
        }
    }
}
