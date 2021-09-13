using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
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
