using Guppy.Events.Delegates;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : Orderable, IEntity
    {
        #region Private Fields
        private ILayer _layer;
        private Int32 _layerGroup;
        #endregion

        #region Public Attributes
        ILayer IEntity.Layer { get => this.Layer; set => this.Layer = value; }

        public ILayer Layer
        {
            get => _layer;
            internal set => this.OnLayerChanged.InvokeIf(value != _layer, this, ref _layer, value);
        }
        public Int32 LayerGroup
        {
            get => _layerGroup;
            set => this.OnLayerGroupChanged.InvokeIf(value != _layerGroup, this, ref _layerGroup, value);
        }
        #endregion

        #region Events
        /// <summary>
        /// Indicates that the current entity has been succesfully added to 
        /// the current LayerGroup value.
        /// </summary>
        public event OnChangedEventDelegate<IEntity, ILayer> OnLayerChanged;

        /// <summary>
        /// Indicates that the current entity wishes to change to another layer.
        /// This will trigger automated processes to update the layer.
        /// </summary>
        public event OnChangedEventDelegate<IEntity, Int32> OnLayerGroupChanged;
        #endregion
    }
}
