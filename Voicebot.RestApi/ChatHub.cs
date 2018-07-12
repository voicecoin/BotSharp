using Amazon.Polly;
using BotSharp.Core.Agents;
using BotSharp.Core.Conversations;
using BotSharp.Core.Engines;
using BotSharp.Core.Engines.Dialogflow;
using BotSharp.Core.Intents;
using BotSharp.Core.Models;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voicebot.Core.TextToSpeech;
using Voicebot.Core.Voicechain;
using Voicebot.RestApi.Agents;

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

            // redirect to third-part api when get fallback intent
            if (aIResponse.Result.Metadata.IntentName == "Default Fallback Intent")
            {
                var apiAi = new ApiAiSDK.ApiAi(new ApiAiSDK.AIConfiguration("d018bf12a8a8419797fe3965637389b0", ApiAiSDK.SupportedLanguage.English));
                var apiAiResponse = apiAi.TextRequest(text);
                aIResponse = apiAiResponse.ToObject<AIResponse>();
            }

            VoicechainResponse<ANameModel> aName = null;
            VoiceId voiceId = dc.Table<AgentVoice>().FirstOrDefault(x => x.AgentId == agentId)?.VoiceId;

            string awsAccessKey = Database.Configuration.GetSection("Aws:AWSAccessKey").Value;
            string awsSecretKey = Database.Configuration.GetSection("Aws:AWSSecretKey").Value;
            var polly = new PollyUtter(awsAccessKey, awsSecretKey);

            await Clients.Caller.SendAsync("HideLoading");

            for (int messageIndex = 0; messageIndex < aIResponse.Result.Fulfillment.Messages.Count; messageIndex++)
            {
                await Clients.Caller.SendAsync("ShowLoading");
                await Task.Delay(1000);

                var message = JObject.FromObject(aIResponse.Result.Fulfillment.Messages[messageIndex]);
                string type = (message["Type"]??message["type"]).ToString();

                if (type == "0")
                {
                    string speech = (message["Speech"]?? message["speech"]).ToString();
                    string filePath = await polly.Utter(speech, Database.ContentRootPath, voiceId);
                    //polly.Play(filePath);

                    await Clients.Caller.SendAsync("ReceiveMessage", new VmTestPayload
                    {
                        ConversationId = conversationId,
                        Sender = rasa.agent.Name,
                        FulfillmentText = speech,
                        Payload = aIResponse,
                        AudioPath = filePath
                    });
                }
                else if (type == "4")
                {
                    var payload = (message["Payload"]?? message["payload"]).ToObject<AIResponseCustomPayload>();
                    if (payload.Task == "delay")
                    {
                        await Task.Delay(int.Parse(payload.Parameters.First().ToString()));
                    }
                    else if (payload.Task == "voice")
                    {
                        //voiceId = VoiceId.FindValue(payload.Parameters.First().ToString());
                    }
                    else if (payload.Task == "terminate")
                    {
                        if (rasa.agent.Id == "fd9f1b29-fed8-4c68-8fda-69ab463da126") continue;

                        // update conversation agent id back to Voiceweb
                        dc.DbTran(() => {
                            var conversation = dc.Table<Conversation>().Find(conversationId);
                            conversation.AgentId = "fd9f1b29-fed8-4c68-8fda-69ab463da126";
                        });

                        await Clients.Caller.SendAsync("Transfer", new VmTestPayload
                        {
                            ConversationId = conversationId,
                            Sender = "Voiceweb",
                            FulfillmentText = "Hi",
                            Payload = aIResponse
                        });
                    }
                    else if (payload.Task == "transfer")
                    {
                        // get VNS, query blockchain
                        var vcDriver = new VoicechainDriver(dc);
                        aName = vcDriver.GetAName(aIResponse.Result.Parameters["VNS"]);

                        // sending feedback when do querying blockchain
                        await Clients.Caller.SendAsync("SystemNotification", new VmTestPayload
                        {
                            ConversationId = conversationId,
                            Sender = rasa.agent.Name,
                            FulfillmentText = $"Querying VNS {aName.Data.Domain} on Blockchain: IP - {aName.Data.Value}. <a href='http://www.voicecoin.net/tx/{aName.Data.Txid}' target='_blank' style='color:white;'>View Transaction</a>",
                            Payload = aName
                        });

                        // switch to another agent
                        var newAgent = (from vns in dc.Table<VnsTable>()
                                        join agent in dc.Table<Agent>() on vns.AgentId equals agent.Id
                                        select new
                                        {
                                            agent.Id,
                                            agent.Name,
                                            vns.Domain
                                        })
                                       .FirstOrDefault(x => x.Domain == aName.Data.Domain);
                        
                        // update conversation agent id to new agent
                        dc.DbTran(() => {
                            var conversation = dc.Table<Conversation>().Find(conversationId);
                            conversation.AgentId = newAgent.Id;
                        });

                        await Clients.Caller.SendAsync("Transfer", new VmTestPayload
                        {
                            ConversationId = conversationId,
                            Sender = newAgent.Name,
                            FulfillmentText = "Hi",
                            Payload = aName
                        });
                    }
                }

                await Clients.Caller.SendAsync("HideLoading");
            }
        }
    }
}
