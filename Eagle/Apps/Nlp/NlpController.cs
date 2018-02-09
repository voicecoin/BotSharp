using Apps.Chatbot.DomainModels;
using Core;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Apps.Nlp
{
    public class NlpController : CoreController
    {
        [HttpGet("pos")]
        public IEnumerable<NlpirSegment> Pos([FromQuery] string text)
        {
            string url = Database.Configuration.GetSection("NlpApi:NlpirUrl").Value + "nlpir/wordsplit/" + text;
            var result = RestHelper.GetSync<NlpirResult>(url);
            return result.WordSplit;
        }
    }
}
