using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Eagle.DomainModels;
using Eagle.DmServices;
using Eagle.Core;
using Eagle.Apps.Chatbot.DomainModels;
using Eagle.Apps.Chatbot.DmServices;
using System.Threading.Tasks;
using Eagle.Utility;
using System.Text.RegularExpressions;

namespace Eagle.Apps.Chatbot.Analyzer
{
    public class AnalyzerController : CoreController
    {
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

            return model.PosTagger(dc).Select(x => new
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

            var segments = model.PosTagger(dc).Select(x => new DmIntentExpressionItem
            {
                Text = x.Text,
                Meta = x.Meta,
                Position = x.Position,
                Length = x.Length
            }).OrderBy(x => x.Position).ToList();

            return segments;
        }
    }
}