using BotSharp.Core.Engines;
using BotSharp.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.UnitTest
{
    [TestClass]
    public class ConversationTest : TestEssential
    {
        [TestMethod]
        public void TextRequestTest()
        {
            var config = new AIConfiguration(BOT_CLIENT_TOKEN, SupportedLanguage.English);
            config.SessionId = Guid.NewGuid().ToString();

            var rasa = new RasaAi(dc, config);

            // Round 1
            var response = rasa.TextRequest(new AIRequest { Query = new String[] { "Hi" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Wakeup");

            // Round 2
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "Voiceweb" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Wakeup");

            // Round 3
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "I'm going to apple store to buy an iPhone." } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Transfer");
            Assert.AreEqual(response.Result.Parameters.First(x => x.Key == "VNS").Value, "Apple Store");
        }
    }
}
