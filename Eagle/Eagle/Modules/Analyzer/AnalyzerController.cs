using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Eagle.Models;
using Eagle.DbContexts;
using Eagle.DddServices;

namespace Eagle.Modules.Analyzer
{
    [Route("v1/Analyzer")]
    public class AnalyzerController : ControllerBase
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
            var model = new AnalyzerModel { Text = text };

            return model.Ner(_context).Select(x => new
            {
                Text = x.Text,
                Entity = x.Alias,
                Position = x.Position,
                Length = x.Length,
                Unit = x.Unit
            }).OrderBy(x => x.Unit);
        }

        [HttpGet("Markup")]
        public IEnumerable<IntentExpressionItemModel> Markup([FromQuery] string text)
        {
            var model = new AnalyzerModel { Text = text };

            return model.Ner(_context).Select(x => new IntentExpressionItemModel
            {
                Text = x.Text,
                EntityId = x.EntityId,
                Position = x.Position,
                Length = x.Length,
                Unit = x.Unit
            }).OrderBy(x => x.Unit);
        }
    }
}