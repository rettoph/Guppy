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
    public class Scene : Driven
    {
        #region Private Fields
        private IServiceScope _scope;
        #endregion

        #region Public Attributes
        public UInt16 DefaultLayerDepth { get; protected set; }
        #endregion

        #region Protected Attributes
        protected Game game { get; private set; }

        protected LayerCollection layers { get; private set; }

        protected EntityCollection entities { get; private set; }
        #endregion

        #region Constructors
        public Scene(IServiceProvider provider) : base(provider)
        {
            // Set the default layer depth
            this.DefaultLayerDepth = 0;
        }
        #endregion

        #region Initialization Methods
        protected override void  Boot()
        {
            base.Boot();

            // Load some required attributes from the service provider
            this.game = provider.GetService<Game>();
            this.layers = provider.GetService<LayerCollection>();
            this.entities = provider.GetService<EntityCollection>();
        }
        #endregion

        #region Frame Methods
        protected override void draw(GameTime gameTime)
        {
            this.layers.Draw(gameTime);
        }

        protected override void update(GameTime gameTime)
        {
            this.layers.Update(gameTime);
        }
        #endregion

        #region Utility Methods
        protected internal void setActive(Boolean active)
        {
            this.Events.TryInvoke("set:active", active);
        }

        protected internal void setScope(IServiceScope scope)
        {
            _scope = scope;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            Entity e;
            while(this.entities.Count() > 0)
            {
                e = this.entities.ElementAt(0);
                this.entities.Remove(e);
                e.Dispose();
            }

            _scope.Dispose();
        }
    }
}
