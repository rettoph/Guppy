using Guppy.UI.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer<TComponent> : IBaseContainer
        where TComponent : IComponent
    {
        ComponentCollection<TComponent> Children { get; }
    }
}
