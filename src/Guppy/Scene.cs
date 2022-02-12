using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions;
using Guppy.EntityComponent.Lists;
using Guppy.Utilities;
using Guppy.Interfaces;
using Guppy.Threading.Utilities;
using System.Threading;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using Guppy.Messages;

namespace Guppy
{
    public abstract class Scene : Asyncable, IDataProcessor<DisposeServiceMessage>
    {
        #region Private Fields
        private ServiceProvider _provider;
        private MessageBus _messageBus;
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
            provider.Service(out _messageBus);
            provider.Service(out _intervals);

            this.Layers = provider.GetService<LayerList>();
            this.Layerables = provider.GetService<LayerableList>();
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _messageBus.GetQueue(Int32.MaxValue).RegisterType<DisposeServiceMessage>();
            _messageBus.RegisterProcessor<DisposeServiceMessage>(this);
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);

            var source = new CancellationTokenSource();
            // TaskHelper.CreateLoop(this.TryUpdate, 16, source.Token);
        }

        protected override void PreUninitialize()
        {
            base.PreUninitialize();

            // When a scene is released lets just dispose the entire ServiceProvider instance.
            // TODO: Verify this cascades and disposes everything
            _provider.Dispose();
        }
        #endregion

        #region Frame Methods 
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Layers.TryDraw(gameTime);
        }

        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            _messageBus.PublishEnqueued(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Layers.TryUpdate(gameTime);

            _intervals.Update(gameTime);
        }
        #endregion

        #region Message Processors
        Boolean IDataProcessor<DisposeServiceMessage>.Process(DisposeServiceMessage message)
        {
            message.Service.Dispose();

            return true;
        }
        #endregion
    }
}
