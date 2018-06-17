using BotSharp.Core.Agents;
using BotSharp.Core.Conversations;
using BotSharp.Core.Engines;
using BotSharp.Core.Models;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string conversationId, string text)
        {
            await Clients.Caller.SendAsync("ShowLoading");

            var dc = new DefaultDataContextLoader().GetDefaultDc();

            string agentId = dc.Table<Conversation>().Find(conversationId).AgentId;
            string clientAccessToken = dc.Table<Agent>().Find(agentId).ClientAccessToken;

            var config = new AIConfiguration(clientAccessToken, SupportedLanguage.English);
            config.SessionId = conversationId;

            var rasa = new RasaAi(dc, config);

            var aIResponse = rasa.TextRequest(new AIRequest { Query = new String[] { text } });

            VoicechainResponse<ANameModel> aName = null;

            await Clients.Caller.SendAsync("HideLoading");

            for (int messageIndex = 0; messageIndex < aIResponse.Result.Fulfillment.Messages.Count; messageIndex++)
            {
                await Clients.Caller.SendAsync("ShowLoading");
                await Task.Delay(500);

                var message = JObject.FromObject(aIResponse.Result.Fulfillment.Messages[messageIndex]);
                string type = message["Type"].ToString();

                if (type == "0")
                {
                    string speech = message["Speech"].ToString();
                    //string filePath = await polly.Utter(speech, env.WebRootPath, voiceId);
                    //polly.Play(filePath);

                    await Clients.Caller.SendAsync("ReceiveMessage", conversationId, speech);
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

                        string t = $"Querying VNS {aName.Data.Domain} on Blockchain: IP - {aName.Data.Value}, Address - {aName.Data.Address}";
                        await Clients.Caller.SendAsync("ReceiveMessage", conversationId, t);
                    }
                }

                await Clients.Caller.SendAsync("HideLoading");
            }
        }
    }
}
