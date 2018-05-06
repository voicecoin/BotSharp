using BotSharp.Core.Conversations;
using BotSharp.Core.Engines;
using BotSharp.Core.Models;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voicebot.Core.Utility;

namespace Voicebot.RestApi.Agents
{
    public class ConversationController : CoreController
    {
        [HttpGet("{agentId}")]
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

        [HttpGet("{conversationId}/{clientAccessToken}")]
        public string Request([FromRoute] string conversationId, [FromRoute] string clientAccessToken, [FromQuery] string text)
        {
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
    }
}
