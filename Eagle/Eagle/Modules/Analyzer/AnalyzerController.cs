using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Eagle.DomainModels;
using Eagle.DbContexts;
using Eagle.DmServices;
using Eagle.Core;

namespace Eagle.Modules.Analyzer
{
    public class AnalyzerController : CoreController
    {
        private readonly DataContexts _context = new DataContexts();

        /// <summary>
        /// NER - 命名实体识别
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        // GET: v1/Analyzer?text=
        [HttpGet("Ner")]
        public IEnumerable<Object> Ner([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            return model.PosTagger(_context).Select(x => new
            {
                Text = x.Text,
                Entity = x.Alias,
                Position = x.Position,
                Length = x.Length
            }).OrderBy(x => x.Position);
        }

        [HttpGet("Markup")]
        public IEnumerable<DmIntentExpressionItem> Markup([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            return model.PosTagger(_context).Select(x => new DmIntentExpressionItem
            {
                Text = x.Text,
                EntityId = x.EntityId,
                Position = x.Position,
                Length = x.Length
            }).OrderBy(x => x.Position);
        }
    }
}