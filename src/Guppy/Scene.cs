﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.EntityComponent.Lists;
using Guppy.Utilities;
using Guppy.Interfaces;
using Guppy.Threading.Utilities;
using Guppy.Utilities.Threading;
using System.Threading;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy
{
    public abstract class Scene : Frameable, IScene
    {
        #region Private Fields
        private ServiceProvider _provider;
        private ThreadQueue _threadQueue;
        private IntervalInvoker _intervals;
        #endregion

        #region Public Properties
        public LayerList Layers { get; private set; }
        public LayerableList Layerables { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            provider.Service(out _threadQueue);
            provider.Service(out _intervals);

            this.Layers = provider.GetService<LayerList>();
            this.Layerables = provider.GetService<LayerableList>();
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            var source = new CancellationTokenSource();
            // TaskHelper.CreateLoop(this.TryUpdate, 16, source.Token);
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
            _threadQueue = null;
            this.Layerables.TryRelease();
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

            _intervals.Update(gameTime);
        }

        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            _threadQueue.Flush(gameTime);
        }
        #endregion
    }
}
