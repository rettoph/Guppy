using Guppy.EntityComponent.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.Interfaces
{
    /// <summary>
    /// An entity is a service capable of containing
    /// components.
    /// </summary>
    public interface IEntity : IService
    {
        ComponentManager Components { get; internal set; }
    }
}
