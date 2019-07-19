using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class LayerCollection : ZFrameableCollection<Layer>
    {
        #region Private Fields
        private IServiceProvider _provider;
        #endregion

        #region Public Attributes
        public Layer this[int index] { get { return this.list[index]; } set { this.list[index] = value; } }
        #endregion

        #region Constructors
        public LayerCollection(IServiceProvider provider)
        {
            _provider = provider;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return the layer of a given layer depth 
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Layer GetLayer(UInt16 depth)
        {
            return this.list.Where(l => l.Configuration.MinDepth <= depth && l.Configuration.MaxDepth >= depth)
                .FirstOrDefault();
        }

        /// <summary>
        /// Create a new instance of a layer, based on the
        /// collection's current scope
        /// </summary>
        /// <typeparam name="TLayer"></typeparam>
        /// <returns></returns>
        public TLayer Create<TLayer>(params Object[] args)
            where TLayer : Layer
        {
            return this.Create<TLayer>(new LayerConfiguration(), args);
        }

        public TLayer Create<TLayer>(LayerConfiguration configuration, params Object[] args)
            where TLayer : Layer
        {
            // Create the new layer
            var layer = _provider.GetRequiredService<LayerFactory<TLayer>>().CreateCustom(_provider, configuration, args);

            // Auto add the new layer
            this.Add(layer);

            // return the new layer
            return layer;
        }

        public TLayer Create<TLayer>(UInt16 minDepth = 0, UInt16 maxDepth = 0, UInt16 updateOrder = 0, UInt16 drawOrder = 0, params Object[] args)
            where TLayer : Layer
        {
            return this.Create<TLayer>(new LayerConfiguration(minDepth, maxDepth, updateOrder, drawOrder), args);
        }
        #endregion

        #region Collection Methods
        public override void Add(Layer item)
        {
            if (this.GetLayer(item.Configuration.MinDepth) != default(Layer))
                throw new Exception($"Unable to add Layer! MinDepth overlap!");
            else if (this.GetLayer(item.Configuration.MaxDepth) != default(Layer))
                throw new Exception($"Unable to add Layer! MaxDepth overlap!");

            // If there is no depth range overlap of the new item... add it to the collection
            base.Add(item);
        }
        #endregion
    }
}
