﻿using Guppy.Collections;
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
        #region Protected Attributes
        protected IServiceProvider provider { get; private set; }

        protected Game game { get; private set; }

        protected LayerCollection layers { get; private set; }
        #endregion

        #region Constructors
        public Scene(IServiceProvider provider) : base(provider.GetService<ILogger>())
        {
            this.provider = provider;
        }
        #endregion

        #region Initialization Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Load some required attributes from the service provider
            this.game = provider.GetService<Game>();
            this.layers = provider.GetService<LayerCollection>();
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
