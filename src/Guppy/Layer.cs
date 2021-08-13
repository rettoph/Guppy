using Guppy.LayerGroups;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Microsoft.Xna.Framework;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Extensions.System.Collections;
using Guppy.Lists.Interfaces;
using Guppy.Interfaces;
using Guppy.Contexts;

namespace Guppy
{
    public class Layer : Orderable, ILayer
    {
        #region Private Fields
        private LayerGroup _group;
        #endregion

        #region Public Attributes
        public OrderableList<ILayerable> Items { get; private set; }
        public LayerGroup Group
        {
            get => _group;
            set
            {
                if (this.Status == Enums.ServiceStatus.Ready)
                    throw new Exception("Unable to update Layer Group post initializtion.");

                _group = value;
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Items = provider.GetService<OrderableList<ILayerable>>();
            this.Items.OnCreated += this.HandleEntityCreated;
        }

        protected override void Release()
        {
            base.Release();

            this.Items.OnCreated -= this.HandleEntityCreated;
            this.Items.TryRelease();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Auto draw all entities within layer
            this.Items.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Auto update all entities within layer
            this.Items.TryUpdate(gameTime);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Automatically update the entities layer to match the current
        /// Layer's group as needed.
        /// </summary>
        /// <param name="item"></param>
        private void HandleEntityCreated(ILayerable item)
        {
            if(!this.Group.Contains(item.LayerGroup))
                item.LayerGroup = this.Group.GetValue();
        }
        #endregion

        #region ILayer Methods
        public void SetContext(LayerContext context)
        {
            this.Group = context.Group;

            base.SetContext(context);
        }
        #endregion
    }
}
