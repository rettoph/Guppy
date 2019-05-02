using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Implementations
{
    public class TrackedDisposable : ITrackedDisposable
    {
        public Boolean Disposed { get; private set; }

        public event EventHandler<ITrackedDisposable> Disposing;

        public virtual void Dispose()
        {
            this.Disposed = true;
            this.Disposing?.Invoke(this, this);
        }
    }
}
