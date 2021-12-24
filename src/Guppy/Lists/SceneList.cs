using Guppy.EntityComponent.DependencyInjection;
using Guppy.Interfaces;
using Guppy.EntityComponent.Lists.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.Lists
{
    public class SceneList : FactoryServiceList<IScene>
    {
        #region Public Attributes
        public IScene Scene { get; private set; }
        #endregion

        #region Events
        /// <inheritdoc/>
        public event Step OnPreDraw;
        /// <inheritdoc/>
        public event Step OnDraw;
        /// <inheritdoc/>
        public event Step OnPostDraw;
        /// <inheritdoc/>
        public event Step OnPreUpdate;
        /// <inheritdoc/>
        public event Step OnUpdate;
        /// <inheritdoc/>
        public event Step OnPostUpdate;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnDraw += this.Draw;
            this.OnUpdate += this.Update;
            this.OnAdd += this.HandleSceneAdded;
        }

        protected override void Release()
        {
            base.Release();

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;
            this.OnAdd -= this.HandleSceneAdded;
        }
        #endregion

        #region Helper Methods
        public void SetScene(IScene scene)
        {
            if (!this.Contains(scene))
                throw new Exception("Unable to render non-child theme.");

            this.Scene = scene;
        }
        #endregion

        #region Create Methods
        protected override T Create<T>(ServiceProvider provider, string serviceName)
        {
            return base.Create<T>(provider.CreateScope(), serviceName);
        }

        protected override T Create<T>(ServiceProvider provider, string serviceName, Action<T, ServiceProvider, ServiceConfiguration> customSetup)
        {
            return base.Create(provider.CreateScope(), serviceName, customSetup);
        }
        #endregion

        #region Frame Methods
        /// <inheritdoc/>
        public void TryDraw(GameTime gameTime)
        {
            this.OnPreDraw?.Invoke(gameTime);
            this.OnDraw?.Invoke(gameTime);
            this.OnPostDraw?.Invoke(gameTime);
        }

        /// <inheritdoc/>
        public void TryUpdate(GameTime gameTime)
        {
            this.OnPreUpdate?.Invoke(gameTime);
            this.OnUpdate?.Invoke(gameTime);
            this.OnPostUpdate?.Invoke(gameTime);
        }
        private void Draw(GameTime gameTime)
        {
            this.Scene?.TryDraw(gameTime);
        }

        private void Update(GameTime gameTime)
        {
            this.Scene?.TryUpdate(gameTime);
        }
        #endregion

        #region Events
        private void HandleSceneAdded(IScene scene)
        {
            if (this.Scene == null)
                this.SetScene(scene);
        }
        #endregion
    }
}
