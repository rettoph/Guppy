using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.EntityComponent.Lists;
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
        private MessageBus _messageBus;
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
            provider.Service(out _messageBus);

            this.Scenes = provider.GetService<SceneList>();
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            _provider.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            _messageBus.PublishEnqueued(gameTime);
        }
        #endregion
    }
}
