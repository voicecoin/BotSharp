using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Utility;
using System.Xml.Linq;
using Core;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.CoreMvcExtension;
using Enyim.Caching;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apps.WeChat
{
    /// <summary>
    /// https://github.com/JeffreySu/WeiXinMPSDK
    /// </summary>
    [AllowAnonymous]
    [Route("/weixin")]
    public class WeixinController : CoreController
    {
        private readonly string Token = "yayaweixin";
        private readonly string EncodingAESKey = "9Rn0jQZ3GgqaVdJKgWTc99U7YSMfb7x95ccBPlHEKA4";
        private readonly string AppId = "wx12b178fb4ffd4560";
        private readonly string AppSecret = "56b83db6271585b9b48cee68fadb9940";

        public WeixinController(IMemcachedClient memcachedClient)
        {
            dc.MemcachedClient = memcachedClient;
        }

        // 灵溪花谷 lingxihuagu wxd4ff56849b9bd433 znSwewfQPsSVX4E0CF69ALTYDXlm3HTlMVygzsUpKPY

        // GET: api/weixin
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return BadRequest("Failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
            }
        }

        // POST: v1/WeChat
        [HttpPost]
        [ActionName("Index")]
        [FormatFilter]
        public async Task<ActionResult> Post(PostModel postModel)
        {
            postModel.Log(MyLogLevel.DEBUG);

            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return BadRequest("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;// 根据自己后台的设置保持一致
            postModel.AppId = AppId;// 根据自己后台的设置保持一致

            var stream = await new StreamReader(Request.Body).ReadToEndAsync();
            XDocument document = XDocument.Parse(stream);
            
            var messageHandler = new WexinMessageHandler(document, postModel);// 接收消息（第一步）
            messageHandler.dc = dc;
            messageHandler.Configuration = Configuration;

            messageHandler.Execute();// 执行微信处理过程（第二步）

            var response = new FixWeixinBugWeixinResult(messageHandler);// 返回（第三步）

            // Fix bug for .net mvc core
            var result = new FixWeixinBugWeixinResult(response.Content);
            result.ContentType = "application/xml";

            return result;
        }
    }
}
