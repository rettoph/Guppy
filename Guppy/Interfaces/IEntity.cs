using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IEntity : IConfigurable
    {
        Int32 LayerDepth { get; set; }

        event EventHandler<Int32> OnLayerDepthChanged;
    }
}
