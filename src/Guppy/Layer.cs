using Guppy.LayerGroups;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Microsoft.Xna.Framework;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Extensions.Collections;
using Guppy.Lists.Interfaces;

namespace Guppy
{
    public class Layer : Orderable
    {
        #region Private Fields
        private LayerGroup _group;
        #endregion

        #region Public Attributes
        public OrderableList<Entity> Entities { get; private set; }
        public LayerGroup Group
        {
            get => _group;
            set
            {
                if (this.InitializationStatus == Enums.InitializationStatus.Ready)
                    throw new Exception("Unable to update Layer Group post initializtion.");

                _group = value;
            }
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entities = provider.GetService<OrderableList<Entity>>();
            this.Entities.OnCreated += this.handleEntityCreated;
        }

        protected override void Release()
        {
            base.Release();

            this.Entities.OnCreated -= this.handleEntityCreated;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Auto draw all entities within layer
            this.Entities.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Auto update all entities within layer
            this.Entities.TryUpdate(gameTime);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Automatically update the entities layer to match the current
        /// Layer's group as needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void handleEntityCreated(Entity item)
        {
            if(!this.Group.Contains(item.LayerGroup))
                item.LayerGroup = this.Group.GetValue();
        }
        #endregion
    }
}
