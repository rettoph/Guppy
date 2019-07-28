using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public abstract class Entity : ZFrameable
    {
        #region Protected Attributes
        protected Scene scene { get; private set; }
        protected EntityCollection entities { get; private set; }
        #endregion

        #region Public Attributes
        public readonly EntityConfiguration Configuration;
        public UInt16 LayerDepth { get; private set; }
        #endregion

        #region Constructors
        public Entity(EntityConfiguration configuration, IServiceProvider provider) : base(provider)
        {
            this.Configuration = configuration;
        }
        public Entity(Guid id, EntityConfiguration configuration, IServiceProvider provider) : base(id, provider)
        {
            this.Configuration = configuration;
        }
        #endregion

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            // Grab global entity objects...
            this.scene = this.provider.GetRequiredService<Scene>();
            this.entities = this.provider.GetRequiredService<EntityCollection>();

            this.SetLayerDepth(this.scene.DefaultLayerDepth); // Set the initial layer depth to the default layer depth
        }
        #endregion

        #region Methods 
        public void SetLayerDepth(UInt16 layerDepth)
        {
            this.LayerDepth = layerDepth;

            this.Events.TryInvoke("changed:layer-depth", this);
        }
        #endregion

        public override void Dispose()
        {
            this.logger.LogDebug($"Destroying Entity<{this.GetType().Name}>({this.Id})");

            base.Dispose();
        }
    }
}
