using Guppy.Events.Delegates;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Layerable : Orderable, ILayerable
    {
        #region Private Fields
        private ILayer _layer;
        private Int32 _layerGroup;
        #endregion

        #region Public Attributes
        ILayer ILayerable.Layer { get => this.Layer; set => this.Layer = value; }

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
        /// <inheritdoc />
        public event OnChangedEventDelegate<ILayerable, ILayer> OnLayerChanged;

        /// <inheritdoc />
        public event OnChangedEventDelegate<ILayerable, Int32> OnLayerGroupChanged;
        #endregion
    }
}
