using Guppy.UI.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IContainer : IBaseContainer
    {
        ComponentCollection Children { get; }
    }
}
