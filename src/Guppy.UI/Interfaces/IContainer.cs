using Guppy.UI.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer<TComponent> : IBaseContainer
        where TComponent : class, IComponent
    {
        ComponentList<TComponent> Children { get; }
    }
}
