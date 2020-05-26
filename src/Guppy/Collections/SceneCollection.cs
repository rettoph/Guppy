using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public sealed class SceneCollection : FactoryCollection<Scene>, IFrameable
    {
        #region Public Attributes
        public Scene Scene { get; private set; }
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
            this.OnAdd += this.AddScene;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnDraw -= this.Draw;
            this.OnUpdate -= this.Update;
            this.OnAdd -= this.AddScene;
        }
        #endregion

        #region Helper Methods
        public void SetScene(Scene scene)
        {
            if (!this.Contains(scene))
                throw new Exception("Unable to render non-child theme.");

            this.Scene = scene;
        }
        #endregion

        #region Factory Methods
        protected override Scene Create(ServiceProvider provider, uint id, Action<Scene, ServiceProvider, ServiceConfiguration> setup = null)
        {
            return base.Create(provider.CreateScope(), id, setup);
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

        #region Collection Methods
        private void AddScene(Scene scene)
        {
            if (this.Scene == null)
                this.SetScene(scene);
        }
        #endregion
    }
}
