using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public interface IEntity : IDisposable
    {
        Guid Id { get; }
        IComponentService Components { get; internal set; }

        event OnEventDelegate<IEntity>? OnDisposed;
    }
}
