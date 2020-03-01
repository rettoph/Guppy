using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ICreatable : IDisposable
    {
        Guid Id { get; }

        event EventHandler OnDisposing;

        void TryCreate(IServiceProvider provider);
    }
}
