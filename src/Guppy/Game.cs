using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.Lists;
using Guppy.Utilities;
using Guppy.Threading.Utilities;
using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// The main Guppy game class. Manages scene instances.
    /// </summary>
    public abstract class Game : Asyncable
    {
        #region Private Fields
        private ThreadQueue _threadQueue;
        private ServiceProvider _provider;
        #endregion

        #region Public Attributes
        public SceneList Scenes { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;
            provider.Service(Constants.ServiceNames.GameUpdateThreadQueue, out _threadQueue);

            this.Scenes = provider.GetService<SceneList>();
        }

        protected override void Release()
        {
            base.Release();

            _threadQueue.TryRelease();
            _threadQueue = null;
        }

        protected override void PostDispose()
        {
            base.PostDispose();

            _provider.Dispose();
            _provider = null;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Scenes.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Scenes.TryUpdate(gameTime);
        }

        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            _threadQueue.Flush(gameTime);
        }
        #endregion
    }
}
