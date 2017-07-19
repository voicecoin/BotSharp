using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Rule
{
    interface IRuleEvent
    {
        IEnumerable<RuleEvent> RegisterEvents();
    }
}
