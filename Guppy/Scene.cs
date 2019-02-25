using Guppy.Collections;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy
{
    public class Scene : LivingObject
    {
        #region Public Attributes
        public UInt16 DefaultLayerDepth { get; protected set; }
        #endregion

        #region Protected Attributes
        protected IServiceProvider provider { get; private set; }

        protected Game game { get; private set; }

        protected LayerCollection layers { get; private set; }

        protected EntityCollection entities { get; private set; }
        #endregion

        #region Constructors
        public Scene(IServiceProvider provider) : base(provider.GetService<ILogger>())
        {
            this.provider = provider;

            // Set the default layer depth
            this.DefaultLayerDepth = 0;
        }
        #endregion

        #region Initialization Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Load some required attributes from the service provider
            this.game = provider.GetService<Game>();
            this.layers = provider.GetService<LayerCollection>();
            this.entities = provider.GetService<EntityCollection>();
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            this.layers.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.layers.Update(gameTime);
        }
        #endregion
    }
}
