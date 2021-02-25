using Guppy.Events.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IEntity : IOrderable
    {
        #region Events
        /// <summary>
        /// Indicates that the current entity has been succesfully added to 
        /// the current LayerGroup value.
        /// </summary>
        event OnChangedEventDelegate<IEntity, ILayer> OnLayerChanged;

        /// <summary>
        /// Indicates that the current entity wishes to change to another layer.
        /// This will trigger automated processes to update the layer.
        /// </summary>
        event OnChangedEventDelegate<IEntity, Int32> OnLayerGroupChanged;
        #endregion

        #region Properties
        public ILayer Layer { get; internal set; }
        public Int32 LayerGroup { get; set; }
        #endregion
    }
}
