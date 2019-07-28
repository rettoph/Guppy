using Guppy.Utilities.DynamicHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IDriven : IFrameable
    {
        EventDelegater Events { get; }
    }
}
