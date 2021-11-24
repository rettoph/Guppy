using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface ILayerable : IOrderable
    {
        #region Events
        /// <summary>
        /// Indicates that the current layerable has been succesfully added to 
        /// the current LayerGroup value.
        /// </summary>
        event OnChangedEventDelegate<ILayerable, ILayer> OnLayerChanged;

        /// <summary>
        /// Indicates that the current layerable wishes to change to another layer.
        /// This will trigger automated processes to update the layer.
        /// </summary>
        event OnChangedEventDelegate<ILayerable, Int32> OnLayerGroupChanged;
        #endregion

        #region Properties
        public ILayer Layer { get; internal set; }
        public Int32 LayerGroup { get; set; }
        #endregion
    }
}
