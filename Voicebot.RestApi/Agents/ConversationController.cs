using BotSharp.Core.Agents;
using BotSharp.Core.Conversations;
using BotSharp.Core.Engines;
using BotSharp.Core.Models;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voicebot.Core.Chatbots.Tuling;
using Voicebot.Core.Utility;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Dialog controller
    /// </summary>
    public class ConversationController : CoreController
    {
        /// <summary>
        /// Start a new conversation, keep contexts.
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpGet("{agentId}/Start")]
        public string Start([FromRoute] string agentId)
        {
            var conversation = dc.Table<Conversation>().FirstOrDefault(x => x.UserId == CurrentUserId && x.AgentId == agentId);
            if (conversation == null)
            {
                dc.DbTran(() => {

                    conversation = new Conversation
                    {
                        AgentId = agentId,
                        UserId = CurrentUserId,
                        StartTime = DateTime.UtcNow
                    };

                    dc.Table<Conversation>().Add(conversation);

                });
            }

            return conversation.Id;
        }

        /// <summary>
        /// Test dialog
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{conversationId}/Test")]
        public new VmTestPayload Test([FromRoute] string conversationId, [FromQuery] string text)
        {
            string agentId = dc.Table<Conversation>().Find(conversationId).AgentId;
            string clientAccessToken = dc.Table<Agent>().Find(agentId).ClientAccessToken;

            var config = new AIConfiguration(clientAccessToken, SupportedLanguage.English);
            config.SessionId = conversationId;

            var rasa = new RasaAi(dc, config);

            var aIResponse = rasa.TextRequest(new AIRequest { Query = new String[] { text } });

            var speeches = new List<String>();
            VoicechainResponse<ANameModel> aName = null;

            for (int messageIndex = 0; messageIndex < aIResponse.Result.Fulfillment.Messages.Count; messageIndex++)
            {
                var message = JObject.FromObject(aIResponse.Result.Fulfillment.Messages[messageIndex]);
                string type = message["Type"].ToString();

                if (type == "0")
                {
                    string speech = message["Speech"].ToString();
                    //string filePath = await polly.Utter(speech, env.WebRootPath, voiceId);
                    //polly.Play(filePath);

                    speeches.Add(speech);
                }
                else if (type == "4")
                {
                    var payload = message["Payload"].ToObject<AIResponseCustomPayload>();
                    if (payload.Task == "delay")
                    {
                        //await Task.Delay(int.Parse(payload.Parameters.First().ToString()));
                    }
                    else if (payload.Task == "voice")
                    {
                        //voiceId = VoiceId.FindValue(payload.Parameters.First().ToString());
                    }
                    else if (payload.Task == "transfer")
                    {
                        // get VNS, query blockchain
                        var vcDriver = new VoicechainDriver(dc);
                        aName = vcDriver.GetAName(aIResponse.Result.Parameters["VNS"]);
                    }
                }
            }

            string fulfillmentText = string.Join(".", speeches);

            return new VmTestPayload
            {
                FulfillmentText = fulfillmentText,
                Payload = aIResponse,
                Voicechain = aName
            };
        }

        /// <summary>
        /// Text request
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="clientAccessToken"></param>
        /// <param name="text">user say</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{conversationId}/{clientAccessToken}")]
        public new string Request([FromRoute] string conversationId, [FromRoute] string clientAccessToken, [FromQuery] string text)
        {
            if (clientAccessToken == "50dbb57981654aa1a6bbf24f612f207f")
            {
                var tuling = new TulingAgent();
                var tulingResponse = tuling.Request(new TulingRequest
                {
                    Perception = new TulingRequestPerception
                    {
                        InputText = new TulingInputText { Text = text }
                    }
                });

                return tulingResponse.Results.FirstOrDefault(x => x.ResultType == "text").Values.Text;
            }

            var config = new AIConfiguration(clientAccessToken, SupportedLanguage.English);
            config.SessionId = conversationId;

            var rasa = new RasaAi(dc, config);

            var aIResponse = rasa.TextRequest(new AIRequest { Query = new String[] { text } });

            var speeches = new List<String>();

            for (int messageIndex = 0; messageIndex < aIResponse.Result.Fulfillment.Messages.Count; messageIndex++)
            {
                var message = JObject.FromObject(aIResponse.Result.Fulfillment.Messages[messageIndex]);
                string type = message["Type"].ToString();

                if (type == "0")
                {
                    string speech = message["Speech"].ToString();
                    //string filePath = await polly.Utter(speech, env.WebRootPath, voiceId);
                    //polly.Play(filePath);

                    speeches.Add(speech);
                }
                else if (type == "4")
                {
                    /*var payload = JsonConvert.DeserializeObject<CustomPayload>(message["payload"].ToString());
                    if (payload.Task == "delay")
                    {
                        await Task.Delay(int.Parse(payload.Parameters.First().ToString()));
                    }
                    else if (payload.Task == "voice")
                    {
                        voiceId = VoiceId.FindValue(payload.Parameters.First().ToString());
                    }*/
                }
            }

            return string.Join(".", speeches);
        }

        /// <summary>
        /// Rest contexts
        /// </summary>
        /// <param name="conversationId"></param>
        [HttpGet("{conversationId}/reset")]
        public string Reset([FromRoute] string conversationId)
        {
            dc.DbTran(() => {

                dc.Table<ConversationContext>()
                    .RemoveRange(dc.Table<ConversationContext>()
                            .Where(x => x.ConversationId == conversationId)
                            .ToList());

                var con = dc.Table<Conversation>().Find(conversationId);
                con.UpdatedTime = DateTime.UtcNow;
            });

            return conversationId;
        }
    }
}
