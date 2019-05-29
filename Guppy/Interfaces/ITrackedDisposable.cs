using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ITrackedDisposable : IDisposable
    {
        Boolean Disposed { get; }

        event EventHandler<ITrackedDisposable> Disposing;
    }
}
