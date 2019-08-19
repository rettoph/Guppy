using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Object that contains a unique ID and custom events.
    /// </summary>
    public interface IUnique : IDisposable
    {
        Guid Id { get; }
        EventDelegater Events { get; }
    }
}
