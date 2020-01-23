using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Orderable
    {
        #region Private Fields
        private LoadedString _name;
        private LoadedString _description;
        #endregion

        #region Public Attributes
        public String Handle { get; set; }
        public String Name { get => _name; set => _name.Set(value); }
        public String Description { get => _description; set => _description.Set(value); }

        public Int32 LayerDepth { get; private set; }
        #endregion

        #region Events & Delegaters 
        public event EventHandler<Int32> OnLayerDepthChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _name = provider.GetRequiredService<LoadedString>();
            _description = provider.GetRequiredService<LoadedString>();
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

                this.OnLayerDepthChanged?.Invoke(this, this.LayerDepth);
            }
        }
        #endregion
    }
}
