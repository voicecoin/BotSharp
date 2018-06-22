using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Agent view model
    /// </summary>
    public class VmAgent
    {
        /// <summary>
        /// Ingore if creating
        /// </summary>
        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Boolean Published { get; set; }

        /// <summary>
        /// zh, en
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Ingore if creating
        /// </summary>
        public String Birthday { get; set; }

        /// <summary>
        /// Image base64 logo
        /// </summary>
        public String Avatar { get; set; }
    }
}
