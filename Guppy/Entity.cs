using Guppy.Configurations;
using Guppy.Utilities.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Orderable
    {
        #region Public Attributes
        public EntityConfiguration Configuration { get; internal set; }
        public Int32 LayerDepth { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Events.Register<Int32>("changed:layer-depth");
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Update the entities layer and invoke the changed:layer event
        /// </summary>
        /// <param name="layer"></param>
        public void SetLayerDepth(Int32 layerDepth)
        {
            if(layerDepth != this.LayerDepth)
            {
                this.LayerDepth = layerDepth;

                this.Events.TryInvoke<Int32>(this, "changed:layer-depth", this.LayerDepth);
            }
        }
        #endregion
    }
}
