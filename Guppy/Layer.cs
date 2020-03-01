using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Xna.Framework;
using Guppy.Utilities.LayerDepths;
using Guppy.Interfaces;

namespace Guppy
{
    public class Layer : Configurable, ILayer
    {
        #region Internal Attributes
        public OrderableCollection<IEntity> Entities { get; private set; }
        public LayerDepth Depth { get; internal set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new entity collection instance
            this.Entities = ActivatorUtilities.CreateInstance<OrderableCollection<IEntity>>(provider);
        }
        #endregion
    }
}
