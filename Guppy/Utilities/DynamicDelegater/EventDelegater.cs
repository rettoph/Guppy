using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Guppy.Utilities.DynamicDelegaters
{
    /// <summary>
    /// Custom events that are managed by the network library.
    /// </summary>
    public class EventDelegater : DynamicDelegater<String, Object>
    {
        public EventDelegater(ILogger logger) : base(logger)
        {
        }
    }
}
