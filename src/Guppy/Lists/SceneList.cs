using DotNetUtils.DependencyInjection;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using GuppyServiceProvider = Guppy.DependencyInjection.GuppyServiceProvider;

namespace Guppy.Lists
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
        protected override void PreInitialize(GuppyServiceProvider provider)
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
