using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    /// <inheritdoc/>
    public class Unique : IUnique
    {
        public Guid Id { get; protected set; }
        public EventDelegater Events { get; private set; }

        public void Dispose()
        {
            // Auto dispose of any self contained event handlers...
            this.Events.Dispose();
        }
    }
}
