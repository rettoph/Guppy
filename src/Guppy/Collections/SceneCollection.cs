using Guppy.DependencyInjection;
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
                throw new Exception("Unable to render non-child theme.");

            this.Scene = scene;
        }
        #endregion

        #region Collection Methods
        protected override void Add(Scene item)
        {
            base.Add(item);

            if (this.Scene == null)
                this.SetScene(item);
        }
        #endregion

        #region Factory Methods
        protected override Scene Create(ServiceProvider provider, uint id)
        {
            return base.Create(provider.CreateScope(), id);
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
