using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Models;

namespace Eagle.Core.Rule
{
    interface IRuleEvent
    {
        IEnumerable<RuleEvent> RegisterEvents();
    }
}
