using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class TrackedDisposable : ITrackedDisposable
    {
        public event EventHandler<ITrackedDisposable> Disposing;

        public virtual void Dispose()
        {
            this.Disposing?.Invoke(this, this);
        }
    }
}
