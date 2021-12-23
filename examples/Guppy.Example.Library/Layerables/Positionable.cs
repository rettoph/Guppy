using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Layerables
{
    /// <summary>
    /// A simple entity capeable of having a position.
    /// </summary>
    public abstract class Positionable : NetworkLayerable
    {
        #region Public Properties
        public virtual Vector2 Position { get; set; }
        #endregion
    }
}
