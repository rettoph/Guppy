using Guppy.Implementations;
using Guppy.Utilities.Configurations;
using Guppy.Utilities.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Driven
    {
        #region Public Attributes
        public EntityConfiguration Configuration { get; internal set; }
        public Layer Layer { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Events.Register<Layer>("changed:layer");
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update the entities layer and invoke the changed:layer event
        /// </summary>
        /// <param name="layer"></param>
        public void SetLayer(Layer layer)
        {
            if(layer != this.Layer)
            {
                this.Layer = layer;

                this.Events.Invoke<Layer>("changed:layer", this, this.Layer);
            }
        }
        #endregion
    }
}
