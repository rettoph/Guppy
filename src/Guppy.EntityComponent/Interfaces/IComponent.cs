using Guppy.EntityComponent.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.Interfaces
{
    /// <summary>
    /// A simple service that can be bound to an entity.
    /// </summary>
    [ManualInitialization]
    public interface IComponent : IService
    {
        IEntity Entity { get; internal set; }
    }
}
