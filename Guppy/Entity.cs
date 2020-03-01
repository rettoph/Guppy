using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Configurable, IEntity
    {
        #region Private Fields
        private Int32 _layerDepth;
        #endregion

        #region
        public Int32 LayerDepth
        {
            get => _layerDepth;
            set
            {
                if (_layerDepth != value)
                {
                    _layerDepth = value;

                    this.OnLayerDepthChanged?.Invoke(this, this.LayerDepth);
                }
            }
        }
        #endregion

        #region Events & Delegaters 
        public event EventHandler<Int32> OnLayerDepthChanged;
        #endregion
    }
}
