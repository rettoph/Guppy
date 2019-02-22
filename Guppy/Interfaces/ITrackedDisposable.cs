using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ITrackedDisposable : IDisposable
    {
        event EventHandler<ITrackedDisposable> Disposing;
    }
}
