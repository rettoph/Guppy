using Guppy.Configurations;
using Guppy.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class LayerCollection : TrackedDisposableCollection<Layer>
    {
        #region Private Fields
        private IServiceProvider _provider;

        private IOrderedEnumerable<Layer> _drawables;
        private IOrderedEnumerable<Layer> _updatables;
        #endregion

        #region Public Attributes
        public Int32 Count { get { return this.list.Count; } }
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
        public TLayer Create<TLayer>()
            where TLayer : Layer
        {
            return this.Create<TLayer>(new LayerConfiguration());
        }

        public TLayer Create<TLayer>(LayerConfiguration configuration)
            where TLayer : Layer
        {
            // Create the new layer
            var layer = _provider.GetLayer<TLayer>(configuration);

            // return the new layer
            return layer;
        }

        public TLayer Create<TLayer>(UInt16 minDepth = 0, UInt16 maxDepth = 0, UInt16 updateOrder = 0, UInt16 drawOrder = 0)
            where TLayer : Layer
        {
            return this.Create<TLayer>(new LayerConfiguration(minDepth, maxDepth, updateOrder, drawOrder));
        }
        #endregion

        #region Frame Methods
        public void Draw(GameTime gameTime)
        {
            // Update all the drawables
            foreach (Layer livingObject in _drawables)
                livingObject.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Update all the updatables
            foreach (Layer livingObject in _updatables)
                livingObject.Update(gameTime);
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

            // When a new item is added we must update the drawables and updatables
            _drawables = this.list.OrderBy(l => l.Configuration.DrawOrder);
            _updatables = this.list.OrderBy(l => l.Configuration.UpdateOrder);
        }

        public override bool Remove(Layer item)
        {
            if(base.Remove(item))
            {
                // When an item is removed we must update the drawables and updatables
                _drawables = this.list.OrderBy(l => l.Configuration.DrawOrder);
                _updatables = this.list.OrderBy(l => l.Configuration.UpdateOrder);

                return true;
            }

            return false;
        }
        #endregion
    }
}
