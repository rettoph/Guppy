using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public sealed class SceneCollection : ServiceCollection<Scene>, IFrameable
    {
        #region Public Attributes
        public Scene Scene { get; private set; }
        #endregion

        #region Helper Methods
        public void SetScene(Scene scene)
        {
            if (!this.Contains(scene))
                throw new Exception("UNable to render non-child theme.");

            this.Scene = scene;
        }
        #endregion

        #region Factory Methods
        protected override Scene Create(ServiceProvider provider, uint id, Action<ServiceProvider, Scene> setup)
        {
            return base.Create(provider.CreateScope(), id, setup);
        }
        #endregion

        #region Frame Methods
        public void TryDraw(GameTime gameTime)
        {
            this.Scene?.TryDraw(gameTime);
        }

        public void TryUpdate(GameTime gameTime)
        {
            this.Scene?.TryUpdate(gameTime);
        }
        #endregion
    }
}
