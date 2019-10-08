using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Xna.Framework;
using Guppy.Utilities.LayerDepths;

namespace Guppy
{
    public class Layer : Orderable
    {
        #region Internal Attributes
        protected internal OrderableCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        /// <summary>
        /// A list of all enabled entities withing the layer ordered by UpdateOrder.
        /// </summary>
        public IEnumerable<Entity> Updates { get { return this.entities.Updates; } }
        /// <summary>
        /// A list of all visible entities within the layer ordered by DrawOrder.
        /// </summary>
        public IEnumerable<Entity> Draws { get { return this.entities.Draws; } }

        /// <summary>
        /// A list of all entities, including enabled and disabled.
        /// </summary>
        public IEnumerable<Entity> Entities { get { return this.entities.AsEnumerable(); } }

        /// <summary>
        /// The current index that the layer resides on.
        /// 
        /// This should be defined when creating a layer
        /// via the LayerCollection create methods.
        /// </summary>
        public LayerDepth Depth { get; internal set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new entity collection instance
            this.entities = ActivatorUtilities.CreateInstance<OrderableCollection<Entity>>(provider);
        }
        #endregion
    }
}
