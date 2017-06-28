using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eagle.Utility;

namespace Eagle.Modules.Analyzer
{
    [Route("v1/Analyzer")]
    public class ConversationController : ControllerBase
    {
        [HttpGet("Text/{sessionId}")]
        public async Task<String> Text([FromRoute] String sessionId, [FromQuery] String text)
        {
            var result = await HttpHelper.Rest<TulingResponse>("http://www.tuling123.com/openapi/api",
                new
                {
                    userid = sessionId,
                    key = "ce36fa6d0ec047248da3354519658734",
                    info = text
                });

           return result.Text;
        }
    }

    public class TulingResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }
}