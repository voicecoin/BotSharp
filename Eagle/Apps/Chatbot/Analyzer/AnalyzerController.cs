using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Core;
using Apps.Chatbot_ConversationParameters.DomainModels;
using Apps.Chatbot_ConversationParameters.DmServices;
using Enyim.Caching;
using Microsoft.AspNetCore.Authorization;

namespace Apps.Chatbot_ConversationParameters.Analyzer
{
    public class AnalyzerController : CoreController
    {
        /// <summary>
        /// NER - 命名实体识别
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        // GET: v1/Analyzer?text=
        [AllowAnonymous]
        [HttpGet("Ner")]
        public IEnumerable<Object> Ner([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            return model.PosTagger(dc)
                .Where(x => !String.IsNullOrEmpty(x.Meta))
                .OrderBy(x => x.Position)
                .Select(x => new
                {
                    Word = x.Text,
                    Entity = x.Alias
                });
        }

        [AllowAnonymous]
        [HttpGet("Pos")]
        public IEnumerable<Object> Pos([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            return model.PosTagger(dc)
                .OrderBy(x => x.Position)
                .Select(x => new
                {
                    Word = x.Text,
                    Entity = x.Alias
                });
        }

        [AllowAnonymous]
        [HttpGet("Markup")]
        public IEnumerable<DmIntentExpressionItem> Markup([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            var segments = model.PosTagger(dc).Select(x => new DmIntentExpressionItem
            {
                Text = x.Text,
                Meta = x.Meta,
                Position = x.Position,
                Length = x.Length,
                Color = x.Color
            }).OrderBy(x => x.Position).ToList();

            return segments;
        }
    }
}