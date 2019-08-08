using Guppy.Collections;
using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace Guppy
{
    /// <summary>
    /// Layers can be added to scenes, and should
    /// contain entities. The layer itself will contain
    /// its own EntityCollection instance and should update
    /// or draw all internal entities as needed.
    /// </summary>
    public abstract class Layer : Driven
    {
        #region Public Attributes
        public LayerEntityCollection Entities { get; private set; }
        public UInt16 Depth { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Create a new layer entity collection
            this.Entities = this.provider.GetService<LayerEntityCollection>();

            // Register custom entity events...
            this.Events.TryRegisterDelegate<UInt16>("changed:depth");
        }
        #endregion

        #region Helper Methods
        public void SetDepth(UInt16 value)
        {
            if (value != this.Depth)
            {
                this.Depth = value;

                this.Events.Invoke<UInt16>("changed:depth", this.Depth);
            }
        }
        #endregion
    }
}
