using Guppy.Events.Delegates;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Orderable
    {
        #region Private Fields
        private Layer _layer;
        private Int32 _layerGroup;
        #endregion

        #region Public Attributes
        public Layer Layer
        {
            get => _layer;
            internal set => this.OnLayerChanged.InvokeIfChanged(value != _layer, this, ref _layer, value);
        }
        public Int32 LayerGroup
        {
            get => _layerGroup;
            set => this.OnLayerGroupChanged.InvokeIfChanged(value != _layerGroup, this, ref _layerGroup, value);
        }
        #endregion

        #region Events
        /// <summary>
        /// Indicates that the current entity has been succesfully added to 
        /// the current LayerGroup value.
        /// </summary>
        public event OnChangedEventDelegate<Entity, Layer> OnLayerChanged;

        /// <summary>
        /// Indicates that the current entity wishes to change to another layer.
        /// This will trigger automated processes to update the layer.
        /// </summary>
        public event OnChangedEventDelegate<Entity, Int32> OnLayerGroupChanged;
        #endregion
    }
}
