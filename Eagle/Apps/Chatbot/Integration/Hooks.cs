using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;

namespace Apps.Chatbot.Integration
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 101;

        public void Load(IHostingEnvironment env, IConfiguration config, Database dc)
        {
            var dm = dc.Table<AgentPlatformEntity>().Add(
                new AgentPlatformEntity
                {
                    AgentId = "",
                    Platform = PlatformType.Wexin,
                    Enable = true,
                    Webhook = "",
                    AppId = "",
                    Token = "",
                    EncodingKey = ""
                });

            dm = dc.Table<AgentPlatformEntity>().Add(
                new AgentPlatformEntity
                {
                    AgentId = "",
                    Platform = PlatformType.Wexin,
                    Enable = true,
                    Webhook = "",
                    AppId = "",
                    Token = "",
                    EncodingKey = ""
                });
        }
    }
}
