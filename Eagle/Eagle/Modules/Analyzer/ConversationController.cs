using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eagle.Utility;
using Eagle.DbContexts;
using Eagle.DmServices;
using Eagle.DomainModels;
using Eagle.Core;

namespace Eagle.Modules.Analyzer
{
    public class ConversationController : CoreController
    {
        private readonly DataContexts _context = new DataContexts();

        [HttpGet("Text")]
        public async Task<String> Text(DmAgentRequest analyzerModel)
        {
            analyzerModel.Log(MyLogLevel.DEBUG);
            // Yaya UserName: gh_0a3fe78f2d13, key: ce36fa6d0ec047248da3354519658734
            // Lingxihuagu UserName: gh_c96a6311ab6d, key: f8bc556e63364c5a8b4e37000d897704

            var agentRecord = _context.Agents.First(x => x.ClientAccessToken == analyzerModel.ClientAccessToken);
            DmAgentRequest agentRequestModel = new DmAgentRequest { Agent = agentRecord.Map<DmAgent>(), Text = analyzerModel.Text };

            var response = agentRequestModel.TextRequest(_context);

            if (response == null || String.IsNullOrEmpty(response.Text))
            {
                var result = await HttpHelper.Rest<TulingResponse>("http://www.tuling123.com/openapi/api",
                    new
                    {
                        userid = analyzerModel.SessionId,
                        key = "ce36fa6d0ec047248da3354519658734",
                        info = analyzerModel.Text
                    });

                result.Log(MyLogLevel.DEBUG);

                return result.Text;
            }
            else
            {
                response.Log(MyLogLevel.DEBUG);

                return response.Text;
            }

        }
    }

    public class TulingResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }
}