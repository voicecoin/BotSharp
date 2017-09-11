using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Apps.Chatbot.Integration
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 101;

        public void Load(IHostingEnvironment env, IConfigurationRoot config, CoreDbContext dc)
        {
            var dm = new DomainModel<AgentPlatformEntity>(dc,
                new AgentPlatformEntity
                {
                    AgentId = "6dfd6dc6-2d63-408a-89cf-ee8ccef24c79",
                    Platform = PlatformType.Wexin,
                    Enable = true,
                    Webhook = "http://api.yaya.ai/weixin/wx12b178fb4ffd4560",
                    AppId = "wx12b178fb4ffd4560",
                    Token = "yayaweixin",
                    EncodingKey = "9Rn0jQZ3GgqaVdJKgWTc99U7YSMfb7x95ccBPlHEKA4"
                });

            dm.AddEntity();

            dm = new DomainModel<AgentPlatformEntity>(dc,
                new AgentPlatformEntity
                {
                    AgentId = "c42cc732-4d50-4c3b-a61a-d643957288df",
                    Platform = PlatformType.Wexin,
                    Enable = true,
                    Webhook = "http://api.yaya.ai/weixin/wxd4ff56849b9bd433",
                    AppId = "wxd4ff56849b9bd433",
                    Token = "yayaweixin",
                    EncodingKey = "znSwewfQPsSVX4E0CF69ALTYDXlm3HTlMVygzsUpKPY"
                });

            dm.AddEntity();
        }
    }
}
