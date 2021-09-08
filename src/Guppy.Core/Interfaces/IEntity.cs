using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// An entity is a service capable of containing
    /// components.
    /// </summary>
    public interface IEntity : IService
    {
        ComponentManager Components { get; set; }
    }
}
