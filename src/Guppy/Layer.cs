using Guppy.Collections;
using Guppy.LayerGroups;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Microsoft.Xna.Framework;

namespace Guppy
{
    public class Layer : Orderable
    {
        #region Private Fields
        private LayerGroup _group;
        #endregion

        #region Public Attributes
        public LayerEntityCollection Entities { get; private set; }
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

            this.Entities = provider.GetService<LayerEntityCollection>((p, i) =>
            { // Create a new LayerEntityCollection bound to this layer.
                i.layer = this;
            });
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
    }
}
