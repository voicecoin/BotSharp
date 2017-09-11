using System;
using System.Threading.Tasks;
using Utility;
using System.Xml.Linq;
using Core;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Context;
using System.Linq;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Apps.Chatbot.Conversation;
using Apps.Chatbot.Agent;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.DmServices;

namespace Apps.WeChat
{
    public class WexinMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CoreDbContext dc { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        public WexinMessageHandler(XDocument inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {

        }

        
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            //ResponseMessageText也可以是News等其他类型
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "默认回复。";
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            requestMessage.Log(MyLogLevel.DEBUG);

            string clientAccessToken = "dcab96ca3f844d029f145b085dabe2c7"; // Default as Yaya

            if (requestMessage.ToUserName.Equals("gh_c96a6311ab6d"))
            {
                clientAccessToken = "4e0cf80467fa4536be57b358a6d54368"; // Lingxihuagu
            }

            var responseMessage = CreateResponseMessage<ResponseMessageText>();

            /*string url = CoreController.Configuration.GetSection("NlpApi:YayabotUrl").Value + "v1/Conversation?clientAccessToken=" + clientAccessToken + "&conversationId=" + requestMessage.FromUserName + "&text=" + requestMessage.Content;

            responseMessage.Content = RestHelper.GetSync(url);*/

            String conversationId = String.Empty;

            var agentRecord = dc.Table<AgentEntity>().First(x => x.ClientAccessToken == clientAccessToken);

            var conversation = dc.Table<ConversationEntity>().FirstOrDefault(x => x.AgentId == agentRecord.Id && x.CreatedUserId == dc.CurrentUser.Id);
            if (conversation == null)
            {
                dc.Transaction<IDbRecord4Core>(delegate
                {
                    var dm = new DomainModel<ConversationEntity>(dc, new ConversationEntity
                    {
                        AgentId = agentRecord.Id
                    });

                    dm.AddEntity();

                    conversationId = dm.Entity.Id;
                });
            }
            else
            {
                conversationId = conversation.Id;
            }

            DmAgentRequest agentRequestModel = new DmAgentRequest { Agent = agentRecord, Text = requestMessage.Content, ConversationId = conversationId };

            var response = agentRequestModel.TextRequest(dc, Configuration.GetSection("NlpApi:NlpirUrl").Value);

            if (response == null || String.IsNullOrEmpty(response.Text))
            {
                var url = Configuration.GetSection("NlpApi:TulingUrl").Value;
                var key = Configuration.GetSection("NlpApi:TulingKey").Value;

                var result = RestHelper.PostSync<TulingResponse>(url,
                    new
                    {
                        userid = conversationId,
                        key = key,
                        info = requestMessage.Content
                    });

                responseMessage.Content = result.Text;
            }
            else
            {
                responseMessage.Content = response.Text;
            }

            /*client.ExecuteAsync(request, (response) => {
                var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText("wx12b178fb4ffd4560", WeixinOpenId, response.Content);
            });

            return DefaultResponseMessage(requestMessage);*/

            return responseMessage;
        }

        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return null;
        }
    }

    public class CustomMessageContext : MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        public CustomMessageContext()
        {
            base.MessageContextRemoved += CustomMessageContext_MessageContextRemoved;
        }

        /// <summary>
        /// 当上下文过期，被移除时触发的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomMessageContext_MessageContextRemoved(object sender, Senparc.Weixin.Context.WeixinContextRemovedEventArgs<IRequestMessageBase, IResponseMessageBase> e)
        {
            /* 注意，这个事件不是实时触发的（当然你也可以专门写一个线程监控）
             * 为了提高效率，根据WeixinContext中的算法，这里的过期消息会在过期后下一条请求执行之前被清除
             */

            var messageContext = e.MessageContext as CustomMessageContext;
            if (messageContext == null)
            {
                return;//如果是正常的调用，messageContext不会为null
            }

            //TODO:这里根据需要执行消息过期时候的逻辑，下面的代码仅供参考

            //Log.InfoFormat("{0}的消息上下文已过期",e.OpenId);
            //api.SendMessage(e.OpenId, "由于长时间未搭理客服，您的客服状态已退出！");
        }
    }
}
