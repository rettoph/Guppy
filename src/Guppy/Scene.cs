using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.Utilities;
using Guppy.Interfaces;

namespace Guppy
{
    public abstract class Scene : Driven, IScene
    {
        #region Private Fields
        private ServiceProvider _provider;
        private Synchronizer _synchronizer;
        #endregion

        #region Public Properties
        public LayerList Layers { get; private set; }
        public EntityList Entities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            provider.Service(out _synchronizer);

            this.Layers = provider.GetService<LayerList>();
            this.Entities = provider.GetService<EntityList>();
        }


        protected override void PreRelease()
        {
            base.PreRelease();

            // When a scene is released lets just dispose the entire ServiceProvider instance.
            _provider.Dispose();
        }

        protected override void Release()
        {
            base.Release();

            _provider = null;
            _synchronizer = null;
            this.Entities.TryRelease();
            this.Layers.TryRelease();
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

        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            _synchronizer.Flush(gameTime);
        }
        #endregion
    }
}
