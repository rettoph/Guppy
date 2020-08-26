using Guppy.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;

namespace Guppy
{
    public abstract class Scene : Driven
    {
        #region Public Attributes
        public LayerCollection Layers { get; private set; }
        public EntityCollection Entities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Layers = provider.GetService<LayerCollection>();
            this.Entities = provider.GetService<EntityCollection>();
        }
        #endregion

        #region Frame Methods 
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Layers.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Layers.TryUpdate(gameTime);
        }
        #endregion
    }
}
