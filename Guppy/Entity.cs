using Guppy.Configurations;
using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Entity : Driven
    {
        #region Public Attributes
        public EntityConfiguration Configuration { get; internal set; }
        public UInt16 LayerDepth { get; private set; }
        #endregion

        #region Initialization Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Register custom entity events...
            this.Events.TryRegisterDelegate<UInt16>("changed:layer-depth");
        }
        #endregion

        #region Helper Method
        public void SetLayerDepth(UInt16 value)
        {
            if(value != this.LayerDepth)
            {
                this.LayerDepth = value;

                this.Events.Invoke<UInt16>("changed:layer-depth", this.LayerDepth);
            }
        }
        #endregion
    }
}
