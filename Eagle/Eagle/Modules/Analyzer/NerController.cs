using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eagle.Models;
using Eagle.DbContexts;
using System.Text.RegularExpressions;
using Eagle.Utility;
using Eagle.Model.Extensions;

namespace Eagle.Modules.Analyzer
{
    [Route("v1/Analyzer")]
    public class NerController : ControllerBase
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
                Alias = x.Alias,
                Position = x.Position,
                Length = x.Length,
                Unit = x.Unit
            }).OrderBy(x => x.Unit);
        }
    }
}