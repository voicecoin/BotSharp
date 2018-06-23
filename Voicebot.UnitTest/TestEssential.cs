using EntityFrameworkCore.BootKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Voicebot.UnitTest
{
    public abstract class TestEssential
    {
#if VOICEWEB_BOT
        public static String BOT_ID = "fd9f1b29-fed8-4c68-8fda-69ab463da126";
        public static String BOT_CLIENT_TOKEN = "d018bf12a8a8419797fe3965637389b0";
        public static String BOT_DEVELOPER_TOKEN = "8553e861eecd4cd7a1c6aff6bdd1cd2f";
        public static String BOT_NAME = "Voiceweb";
#elif APPLESTORE_BOT
        public static String BOT_ID = "a9fff0df-3136-4114-a152-8e9654f0794a";
        public static String BOT_CLIENT_TOKEN = "76bff41aa5374605b0a3e8a1dab2e284";
        public static String BOT_DEVELOPER_TOKEN = "4002d9d2d2c845fb87950796b8a5757d";
        public static String BOT_NAME = "AppleStore";
#endif

        protected Database dc { get; set; }
        protected string contentRoot;

        public TestEssential()
        {
            contentRoot = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\Voicebot.WebStarter\\";

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var settings = Directory.GetFiles(contentRoot + "/Settings/", "*.json");
            settings.ToList().ForEach(setting =>
            {
                configurationBuilder.AddJsonFile(setting, optional: false, reloadOnChange: true);
            });
            Database.Configuration = configurationBuilder.Build();

            Database.Assemblies = new String[] { "BotSharp.Core", "Voicebot.Core" };
            Database.ContentRootPath = contentRoot;

            dc = new DefaultDataContextLoader().GetDefaultDc();
        }
    }
}
