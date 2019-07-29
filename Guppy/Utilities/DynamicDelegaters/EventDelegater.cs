using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Utilities.DynamicDelegaters
{
    /// <summary>
    /// Custom events that are managed by Guppy.
    /// </summary>
    public class EventDelegater : DynamicDelegater<String, Object>
    {
        public EventDelegater(ILogger logger, Object sender = null) : base(logger, sender)
        {
        }
    }
}
