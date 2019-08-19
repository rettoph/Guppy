using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Utilities.Delegaters
{
    public class EventDelegater : Delegater<String, Object>
    {
        public EventDelegater(ILogger logger) : base(logger)
        {
        }
    }
}
