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

        #region Events
        /// <summary>
        /// Called when the current scene is marked as the active scene.
        /// </summary>
        public event EventHandler<Scene> OnActiveSet;
        /// <summary>
        /// Called when the current scene is no longer the active scene.
        /// </summary>
        public event EventHandler<Scene> OnActiveRemoved;
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
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.layers.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.layers.Update(gameTime);
        }
        #endregion

        #region Utility Methods
        protected internal void setActive(Boolean active)
        {
            if (active)
                this.OnActiveSet?.Invoke(this, this);
            else
                this.OnActiveRemoved?.Invoke(this, this);
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
