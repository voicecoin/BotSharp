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
        public void YayaTest()
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

        [TestMethod]
        public void AppleStoreTest()
        {
            var config = new AIConfiguration(BOT_CLIENT_TOKEN, SupportedLanguage.English);
            config.SessionId = Guid.NewGuid().ToString();

            var rasa = new RasaAi(dc, config);

            // Round 1
            var response = rasa.TextRequest(new AIRequest { Query = new String[] { "Hi" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Default Fallback Intent");

            // Round 2
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "I want to buy an iPhone 10" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Buy iPhone");
            Assert.AreEqual(response.Result.Parameters.First(x => x.Key == "iPhone").Value, "iPhone 10");
            Assert.AreEqual(response.Result.Contexts.FirstOrDefault(x => x.Name == "askshippingaddress").Lifespan, 5);

            // ask shipping address
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "1787 Orchard Ln, Northfield, IL 60093" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Telling Shipping Address");
            //Assert.AreEqual(response.Result.Parameters.FirstOrDefault(x => x.Key == "address").Value, "");

            // ask if payment
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "Yes" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Place work order");
            //Assert.AreEqual(response.Result.Parameters.FirstOrDefault(x => x.Key == "VNS").Value, "Apple Store");

            // end conversation
            response = rasa.TextRequest(new AIRequest { Query = new String[] { "Have a good one" } });
            Assert.AreEqual(response.Result.Metadata.IntentName, "Default Fallback Intent");
        }
    }
}
