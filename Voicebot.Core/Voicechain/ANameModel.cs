using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Voicechain
{
    public class ANameModel
    {
        public string Txid { get; set; }

        public int Time { get; set; }

        public int Height { get; set; }

        public string Address { get; set; }

        [JsonProperty("address_is_mine")]
        public bool AddressIsMine { get; set; }

        public string Operation { get; set; }

        [JsonProperty("days_added")]
        public int DaysAdded { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }
    }
}
