using Guppy.Configurations;
using Guppy.Implementations;
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
        #endregion

        #region Public Attributes
        public readonly EntityConfiguration Configuration;
        public UInt16 LayerDepth { get; private set; }
        #endregion

        #region Events
        public event EventHandler<Entity> OnLayerDepthChanged;
        #endregion

        #region Constructors
        public Entity(EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(provider, logger)
        {
            this.Configuration = configuration;

            this.scene = scene; // Save the entities scene
            this.SetLayerDepth(this.scene.DefaultLayerDepth); // Set the initial layer depth to the default layer depth
        }
        public Entity(Guid id, EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(id, provider, logger)
        {
            this.Configuration = configuration;

            this.scene = scene; // Save the entities scene
            this.SetLayerDepth(this.scene.DefaultLayerDepth); // Set the initial layer depth to the default layer depth
        }
        #endregion

        #region Methods 
        public void SetLayerDepth(UInt16 layerDepth)
        {
            this.LayerDepth = layerDepth;

            this.OnLayerDepthChanged?.Invoke(this, this);
        }
        #endregion
    }
}
