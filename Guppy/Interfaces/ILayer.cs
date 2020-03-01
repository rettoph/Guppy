using Guppy.Collections;
using Guppy.Utilities.LayerDepths;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILayer : IConfigurable
    {
        OrderableCollection<IEntity> Entities { get; }

        /// <summary>
        /// The current index that the layer resides on.
        /// 
        /// This should be defined when creating a layer
        /// via the LayerCollection create methods.
        /// </summary>
        LayerDepth Depth { get; }
    }
}
