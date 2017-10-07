using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Nlp
{
    public enum NlpEngine
    {
        /// <summary>
        /// Standford
        /// </summary>
        CoreNlp = 1,
        /// <summary>
        /// 波森
        /// </summary>
        Boson = 2,
        /// <summary>
        /// 语言云
        /// </summary>
        LtpCloud = 3,
        /// <summary>
        /// 结巴分词
        /// </summary>
        Jieba = 4,
        /// <summary>
        /// 科大讯飞语言云
        /// </summary>
        VoiceCloud = 5,
        /// <summary>
        /// 中文版维基百科实体库
        /// </summary>
        CnDbPedia = 6,
        YayaAi = 8
    }

    public enum CacheType
    {
        Knowledge = 1,
        Question = 2
    }
}
