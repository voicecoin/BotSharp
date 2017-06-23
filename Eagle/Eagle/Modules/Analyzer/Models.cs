using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Models
{
    public class DeepParsedModel
    {
        public List<IntentExpressionItemModel> Tags { get; set; }
    }
}
